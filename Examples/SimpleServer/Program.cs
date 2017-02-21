using System;
using TcpTubes;

namespace SimpleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Open Client
            Console.WriteLine("Server is waiting for connection\nPress any key to transmit it. Press ctrl+c to exit...");
            var server = new TcpServerHub(2);

            // Exit when key is pressed
            while (true)
            {
                // Receive messages
                var msg = server.GetMessage(100);
                if (msg != null)
                    Console.WriteLine($"Message '{msg.MessageId}' received from #{msg.SourceId} (payload is {msg.Payload?.Length ?? 0} bytes long)");

                // Send key strokes
                if (Console.KeyAvailable)
                {
                    var key = (byte)Console.ReadKey().KeyChar;
                    server.SendMessage("KeyPressed", new[] { key });
                }
            }
        }
    }
}
