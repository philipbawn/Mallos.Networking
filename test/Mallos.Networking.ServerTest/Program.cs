namespace Mallos.Networking.ServerTest
{
    using System.Threading;
    using Microsoft.Extensions.Configuration;

    class Program
    {
        static void Main(string[] args)
        {
            // Lets wait here until the server has started.
            Thread.Sleep(1000);

            var serviceProvider = new ServicesCollection();
            serviceProvider.AddService(typeof(IConfiguration), new ConfigurationBuilder().Build());

            var server = new NetServer(serviceProvider);

            Helpers.WaitUntilRunning(server);

            while (server.Running)
            {
                Thread.Sleep(100);
            }
        }
    }
}
