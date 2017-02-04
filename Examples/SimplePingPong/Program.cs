using System;
using System.Threading.Tasks;
using TcpTubes;

namespace SimplePingPong
{
    class Program
    {
        static void Main(string[] args)
        {
            // count the number of pings around
            var msgCounter = 0;

            // Create server
            var server = new TcpServerHub(1, "0.0.0.0", 1222);
            Task.Run(() =>
            {
                while (true)
                {
                    // Get server message
                    uint sourceId; string msgId; byte[] data;
                    if (server.GetMessage(out sourceId, out msgId, out data, int.MaxValue))
                    {
                        msgCounter++;
                        if (msgId == "ping" || msgId == "#connected")
                            server.SendMessage(sourceId, "ping", null);
                    }
                }
            });

            // Create client
            var client = new TcpClientHub(2, "127.0.0.1", 1222);
            Task.Run(() =>
            {
                while (true)
                { 
                    // Get client message
                    uint sourceId; string msgId; byte[] data;
                    if (client.GetMessage(out sourceId, out msgId, out data, int.MaxValue))
                    {
                        msgCounter++;
                        if (msgId == "ping")
                            client.SendMessage("ping", null);
                    }
                }
            });

            // Message/Sec reporter
            Task.Run(() =>
            {
                while (true)
                {
                    var prevCounter = msgCounter;
                    Task.Delay(1000).Wait();
                    Console.WriteLine("Msg/Sec: " + (msgCounter - prevCounter));
                }
            });

            // Wait...
            Console.ReadKey();

            // Shutdown everything
            server.Close();
            client.Close();
        }
    }
}
