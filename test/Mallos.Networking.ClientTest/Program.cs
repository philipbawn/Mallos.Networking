namespace Mallos.Networking.ClientTest
{
    using System.Threading;
    using Microsoft.Extensions.Configuration;

    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServicesCollection();
            serviceProvider.AddConfig(args);

            var client = new NetClient(serviceProvider);

            client.Start(new NetConnectionParameters("", "", "127.0.0.1"));

            Helpers.WaitUntilRunning(client);
            while (client.Running)
            {
                client.SendMessage("Hello World");

                Thread.Sleep(100);
            }
        }
    }
}
