namespace Mallos.Networking.ClientTest
{
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = Services.Create(args);

            var client = new NetClient(serviceProvider);

            client.Start(new NetConnectionParameters("eric", "", "localhost"));

            Helpers.WaitUntilRunning(client);
            while (client.Running)
            {
                client.Chat.SendMessage("Hello World");

                Thread.Sleep(1000);
            }
        }
    }
}
