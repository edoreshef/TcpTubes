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
        /// <summary>
        /// Use WaitForever to block GetMessage until a message arrives
        /// </summary>
        public const int WaitForever = int.MaxValue;

        /// <summary>
        /// Id of the local hub
        /// </summary>
        public uint LocalId { get; set; }

        private Dictionary<uint, Tube> m_Tubes = new Dictionary<uint, Tube>();

        private BlockingCollection<Message> m_ReceiveQueue = new BlockingCollection<Message>();

        /// <summary>
        /// A list of connected clients
        /// </summary>
        public uint[] ConnectedClients { get; private set; } = new uint[0];

        protected void CreatePipe(NetworkStream stream)
        {
            new Tube(LocalId, stream, m_ReceiveQueue);
        }

        public Hub()
        {
        }

        public void Close()
        {
            // Start by not accepting new tubes
            StopAcceptingTubes();

            // Close existing tubes
            foreach (var tube in SafeGetTubeList())
                tube.Value.Close();

            // Append '#terminated' message to queue
            m_ReceiveQueue.Add(new Message { MessageId = "#terminated" });
        }

        public virtual Capsule GetMessage(int timeOut = 0)
        {
            // Take care of house keeping
            foreach (var tube in SafeGetTubeList())
                tube.Value.HouseKeeping();

            // Try to pull message
            Message msg;
            if (!m_ReceiveQueue.TryTake(out msg, timeOut))
                return null;

            // Is it a '#Connected' message?
            if (msg.MessageId == InternalMessages.Connected)
                lock (m_Tubes)
                {
                    // Add tube to list
                    m_Tubes[msg.Source.RemoteId] = msg.Source; 

                    // Rebuild connected client list
                    ConnectedClients = m_Tubes.Select(t => t.Key).ToArray();
                }

            // Is it a '#Disconnected' message?
            if (msg.MessageId == InternalMessages.Disconnected)
                lock (m_Tubes)
                {
                    // Remove tube from list
                    m_Tubes.Remove(msg.Source.RemoteId);

                    // Rebuild connected client list
                    ConnectedClients = m_Tubes.Select(t => t.Key).ToArray();
                }

            // return capsule
            return new Capsule
            {
                SourceId = msg.Source?.RemoteId ?? LocalId,
                MessageId = msg.MessageId,
                Payload = msg.Payload
            };
        }

        public void SendMessage(string messageId, byte[] payload)
        {
            // Broadcast message
            foreach (var tube in SafeGetTubeList())
                tube.Value.SendMessage(messageId, payload);
        }

        public void SendMessage(uint targetId, string messageId, byte[] payload)
        {
            // Get tube
            Tube tube;
            lock (m_Tubes)
                if (!m_Tubes.TryGetValue(targetId, out tube))
                    return;

            // Send message
            tube.SendMessage(messageId, payload);
        }

        protected virtual void StopAcceptingTubes()
        {
        }

        private KeyValuePair<uint, Tube>[] SafeGetTubeList()
        {
            lock (m_Tubes)
                return m_Tubes.ToArray();
        }
    }

    public class TcpServerHub : Hub
    {
        private IPAddress m_InterfaceIP;
        private int       m_ListenPort;

        private ManualResetEvent m_Terminated = new ManualResetEvent(false);
        private TcpListener      m_TcpListener;
        private Thread           m_ListenThread;

        public TcpServerHub(string interfaceIP = "0.0.0.0", int listenPort = 1222)
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

                    // Configure client connection
                    client.NoDelay = true;

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

        public TcpClientHub(string serverAddress, int serverPort)
        {
            // Save adapter and port
            m_ServerAddress = serverAddress;
            m_ServerPort    = serverPort;

            // By default servers ID is '1000'
            LocalId = 1000;

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

                    // Configure client connection
                    if (LocalId == 0)
                    {
                        LocalId = ((IPEndPoint) client.Client.LocalEndPoint).Address.GetAddressBytes().Last();
                    }
                    client.NoDelay = true;

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

        public override Capsule GetMessage(int timeOut = 0)
        {
            var capsule = base.GetMessage(timeOut);

            // Track disconnected status
            if (capsule?.MessageId == "#disconnected")
                m_Connected = false;

            return capsule;
        }
    }
}
