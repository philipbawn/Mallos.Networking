namespace Mallos.Networking.ClientTest
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();
        static async Task MainAsync(string[] args)
        {
            var serviceProvider = Services.Create(args);

            var client = new NetClient(serviceProvider);

            AddEvents(client);

            await client.Start(new NetConnectionParameters("eric", "abc123", "localhost"));

            while (client.Status != NetPeerStatus.Offline)
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
