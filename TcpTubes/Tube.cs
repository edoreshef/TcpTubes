using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpTubes
{
    internal class Tube
    {
        public uint LocalId { get; }
        public uint RemoteId { get; private set; }

        private BlockingCollection<Message> m_ReceiveQueue;
        private NetworkStream m_NetworkStream;
        private Thread m_ReceiveThread;
        private Timer m_KeepaliveSender;
        private int m_LastReceivedMessage = Environment.TickCount;
        private object m_WriterLock = new object();

        public Tube(uint localId, NetworkStream networkStream, BlockingCollection<Message> receiveQueue)
        {
            // Store parameters
            LocalId = localId;
            m_NetworkStream = networkStream;
            m_ReceiveQueue = receiveQueue;

            // Send '#Hello' message
            SendMessage("#hello", BitConverter.GetBytes(LocalId));

            // Start receive thread
            m_ReceiveThread = new Thread(ReceiveThread);
            m_ReceiveThread.IsBackground = true;
            m_ReceiveThread.Start();

            // Start keepalive timer
            m_KeepaliveSender = new Timer(state => SendMessage("#keepalive", null), null, 0, 5000);
        }

        public void SendMessage(string msgId, byte[] msgData)
        {
            try
            {
                // Take care of default values
                if (msgData == null)
                    msgData = new byte[0];

                // Encode header
                var header = new byte[256];
                header[0] = (byte) Encoding.UTF8.GetBytes(msgId, 0, msgId.Length, header, 1);
                header[1 + header[0] + 0] = (byte) (msgData.Length >> 0);
                header[1 + header[0] + 1] = (byte) (msgData.Length >> 8);
                header[1 + header[0] + 2] = (byte) (msgData.Length >> 16);
                header[1 + header[0] + 3] = (byte) (msgData.Length >> 24);

                // Write protection
                lock (m_WriterLock)
                {
                    // Write header
                    m_NetworkStream.Write(header, 0, 1 + header[0] + 4);

                    // Write data
                    m_NetworkStream.Write(msgData, 0, msgData.Length);
                }
            }
            catch (Exception)
            {
                // Do nothing, let receive thread terminate and notify on error
            }
        }

        public void HouseKeeping()
        {
            // Make sure we're alive
            if (m_ReceiveThread == null || !m_ReceiveThread.IsAlive)
                return;

            // Check for timeout
            if (Environment.TickCount - m_LastReceivedMessage > 6000)
                Close();
        }

        public void Close()
        {
            // Stop sending keep alive
            m_KeepaliveSender.Dispose();
            m_KeepaliveSender = null;

            // Try to close network stream 
            try
            {
                m_NetworkStream?.Close();
            }
            catch (Exception)
            {
                // Do nothing.
            }

            // Wait for thread to terminate
            m_ReceiveThread.Join();
        }

        private void ReceiveThread()
        {
            try
            {
                var header = new byte[256];

                while (true)
                {
                    // read messageId size
                    var headerReadCount = m_NetworkStream.Read(header, 0, 1);
                    if (headerReadCount == 0)
                        return;

                    // read entire header
                    var headerSize = 1 + header[0] + 4;
                    while (headerReadCount < headerSize)
                    {
                        // read header size
                        var readCount = m_NetworkStream.Read(header, headerReadCount, headerSize - headerReadCount);
                        if (readCount == 0)
                            return;

                        // Update header read size
                        headerReadCount += readCount;
                    }

                    // Decode header
                    var msgId = Encoding.UTF8.GetString(header, 1, header[0]);
                    var dataLength = BitConverter.ToUInt32(header, 1 + header[0]);

                    // Build message
                    var msg = new Message
                    {
                        Source = this,
                        MessageId = msgId,
                        Data = new byte[dataLength]
                    };

                    // read entire data
                    var dataSize = 0;
                    while (dataSize < dataLength)
                    {
                        // read data
                        var readCount = m_NetworkStream.Read(msg.Data, dataSize, (int) dataLength - dataSize);
                        if (readCount == 0)
                            return;

                        // Update header read size
                        dataSize += readCount;
                    }

                    // MESSAGE RECEVIED! Remember the current time for keepalive testing
                    m_LastReceivedMessage = Environment.TickCount;

                    // is it an '#hello' message?
                    if (msgId == "#hello")
                    {
                        // Extract remote Id
                        RemoteId = BitConverter.ToUInt32(msg.Data, 0);

                        // Add connected to queue
                        m_ReceiveQueue.Add(new Message {MessageId = "#connected", Source = this});
                    }
                    else if (msgId == "#keepalive")
                    {
                        // Do nothing...
                    }
                    else
                    {
                        // Make sure we got an '#Hello' message already
                        if (RemoteId != uint.MaxValue)
                            m_ReceiveQueue.Add(msg);
                    }
                }
            }
            catch (Exception)
            {
                // Do nothing
            }
            finally
            {
                // Signal the end
                m_ReceiveQueue.Add(new Message { MessageId = "#disconnected", Source = this });
            }
        }
    }

    internal class Message
    {
        public Tube Source;
        public string MessageId;
        public byte[] Data;
    }
}