using System;
using System.Threading.Tasks;
using TcpTubes;

namespace EchoPerfTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // count the number of pings around
            var msgCounter = 0;

            //
            // Create server
            //
            var server = new TcpServerHub(1);
            Task.Run(() =>
            {
                while (true)
                {
                    // Get server message
                    var msg = server.GetMessage(Hub.WaitForever);

                    // Count incoming messages
                    msgCounter++;

                    // Send respnse (+kickstart on #connected)
                    if (msg.MessageId == "pong" || msg.MessageId == "#connected")
                        server.SendMessage(msg.SourceId, "ping", null);
                }
            });

            //
            // Create client
            //
            var client = new TcpClientHub(2, "127.0.0.1", 1222);
            Task.Run(() =>
            {
                while (true)
                {
                    // Get client message
                    var msg = client.GetMessage(Hub.WaitForever);

                    // Count incoming messages
                    msgCounter++;

                    // Send response
                    if (msg.MessageId == "ping")
                        client.SendMessage("pong", null);
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
