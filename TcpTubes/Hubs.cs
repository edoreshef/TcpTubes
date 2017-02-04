using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TcpTubes
{
    public class Hub
    {
        public uint LocalId { get; }

        private Dictionary<uint, Tube> m_Tubes = new Dictionary<uint, Tube>();

        private BlockingCollection<Message> m_ReceiveQueue = new BlockingCollection<Message>();

        protected void CreatePipe(NetworkStream stream)
        {
            new Tube(LocalId, stream, m_ReceiveQueue);
        }

        public Hub(uint localId)
        {
            LocalId = localId;
        }

        public void Close()
        {
            // Start by not accepting new tubes
            StopAcceptingTubes();

            // Close existing tubes
            foreach (var tube in m_Tubes.ToArray())
                tube.Value.Close();

            // Append '#terminated' message to queue
            m_ReceiveQueue.Add(new Message { MessageId = "#terminated" });
        }

        public virtual bool GetMessage(out uint sourceId, out string messageId, out byte[] data, int timeOut = 0)
        {
            // Default outputs
            sourceId = 0;
            messageId = "";
            data = null;

            // Take care of house keeping
            foreach (var tube in m_Tubes)
                tube.Value.HouseKeeping();

            // Try to pull message
            Message msg;
            if (!m_ReceiveQueue.TryTake(out msg, timeOut))
                return false;

            // Is it a '#Connected' message?
            if (msg.MessageId == "#connected")
                m_Tubes[msg.Source.RemoteId] = msg.Source; // Add tube to list

            // Is it a '#Disconnected' message?
            if (msg.MessageId == "#disconnected")
                m_Tubes.Remove(msg.Source.RemoteId); // Remove tube from list

            // Build return values
            sourceId = msg.Source?.RemoteId ?? LocalId;
            messageId = msg.MessageId;
            data = msg.Data;

            // Return data
            return true;
        }

        public void SendMessage(string messageId, byte[] data)
        {
            // Broadcast message
            foreach (var tube in m_Tubes)
                tube.Value.SendMessage(messageId, data);
        }

        public void SendMessage(uint targetId, string messageId, byte[] data)
        {
            Tube tube;
            if (m_Tubes.TryGetValue(targetId, out tube))
                tube.SendMessage(messageId, data);
        }

        protected virtual void StopAcceptingTubes()
        {
        }
    }

    public class TcpServerHub : Hub
    {
        private IPAddress m_InterfaceIP;
        private int       m_ListenPort;

        private ManualResetEvent m_Terminated = new ManualResetEvent(false);
        private TcpListener m_TcpListener;
        private Thread    m_ListenThread;


        public TcpServerHub(uint localId, string interfaceIP = "0.0.0.0", int listenPort = 1222): base(localId)
        {
            // Save adapter and port
            m_InterfaceIP = IPAddress.Parse(interfaceIP);
            m_ListenPort = listenPort;

            // Start thread
            m_ListenThread = new Thread(ListenThread);
            m_ListenThread.IsBackground = true;
            m_ListenThread.Start();
        }

        private void ListenThread()
        {
            Thread.CurrentThread.Name = "PipeServer_Listen";
            try
            {
                // TcpListener server = new TcpListener(port);
                m_TcpListener = new TcpListener(m_InterfaceIP, m_ListenPort);

                // Start listening for client requests.
                m_TcpListener.Start();

                // Enter the listening loop.
                while (!m_Terminated.WaitOne(10))
                {
                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    var client = m_TcpListener.AcceptTcpClient();

                    // Start pipe
                    CreatePipe(client.GetStream());
                }
            }
            catch (Exception)
            {
                // Shit happens
            }
            finally
            {
                // Stop listening for new clients.
                m_TcpListener?.Stop();
            }
        }

        protected override void StopAcceptingTubes()
        {
            // Close thread
            m_Terminated.Set();
            m_TcpListener.Stop();
            m_ListenThread.Join();
        }

    }

    public class TcpClientHub : Hub
    {
        private string m_ServerAddress;
        private int    m_ServerPort;
        private bool   m_Connected;

        private Thread m_ConnectThread;
        private ManualResetEvent m_Terminated = new ManualResetEvent(false);

        public TcpClientHub(uint localId, string serverAddress, int serverPort): base(localId)
        {
            // Save adapter and port
            m_ServerAddress = serverAddress;
            m_ServerPort    = serverPort;

            // Start thread
            m_ConnectThread = new Thread(ConnectingThread);
            m_ConnectThread.IsBackground = true;
            m_ConnectThread.Start();
        }

        private void ConnectingThread()
        {
            Thread.CurrentThread.Name = "PipeClient_Connect";
            TcpClient client = null;
         
            // Enter the reconnecting loop
            while (true)
            {
                // Sleep + check for termination
                if (m_Terminated.WaitOne(1000))
                    return;

                // Already connected? do nothing              
                if (m_Connected)
                    continue;

                // Make sure Old connection is closed
                try { client?.Close(); } catch (Exception) { }
                client = null;

                // Create new connection and try to connect it
                try
                {
                    client = new TcpClient();
                    if (!client.ConnectAsync(m_ServerAddress, m_ServerPort).Wait(2000))
                        continue;

                    // Remember that we're connect so we wont reconnect again
                    m_Connected = true;

                    // Start pipe
                    CreatePipe(client.GetStream());
                }
                catch (Exception)
                {
                    // Do nothing... Just try again
                }
            }
        }

        protected override void StopAcceptingTubes()
        {
            // Close thread
            m_Terminated.Set();
            m_ConnectThread.Join();
        }

        public override bool GetMessage(out uint sourceId, out string messageId, out byte[] data, int timeOut = 0)
        {
            var retVal = base.GetMessage(out sourceId, out messageId, out data, timeOut);

            // Track disconnected status
            if (messageId == "#disconnected")
                m_Connected = false;

            return retVal;
        }
    }
}
