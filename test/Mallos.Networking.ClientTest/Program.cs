namespace Mallos.Networking.ClientTest
{
    using System;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = Services.Create(args);

            var client = new NetClient(serviceProvider);

            AddEvents(client);

            var connectResult = client.Start(new NetConnectionParameters("eric", "abc123", "localhost")).GetAwaiter().GetResult();

            while (client.Status == NetPeerStatus.Online)
            {
                client.Chat.SendMessage("Hello World");

                Thread.Sleep(1000);
            }
        }

        static void AddEvents(NetClient client)
        {
            client.Chat.Received += (message) =>
            {
                Console.WriteLine(message.ToString());
            };
        }
    }
}
