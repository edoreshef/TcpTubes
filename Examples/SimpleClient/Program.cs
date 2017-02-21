using System;
using TcpTubes;

namespace SimpleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Open Client
            Console.WriteLine("Client is trying to connect to 127.0.0.1:1234.\nPress any key to transmit it. Press ctrl+c to exit...");
            var client = new TcpClientHub(1, "127.0.0.1", 1222);

            // Exit when key is pressed
            while (true)
            {
                // Receive messages
                var msg = client.GetMessage(100);
                if (msg != null)
                    Console.WriteLine($"Message '{msg.MessageId}' received from #{msg.SourceId} (payload is {msg.Payload?.Length ?? 0} bytes long)");

                // Send key strokes
                if (Console.KeyAvailable)
                {
                    var key = (byte)Console.ReadKey().KeyChar;
                    client.SendMessage("KeyPressed", new[] { key });
                }
            }
        }
    }
}
