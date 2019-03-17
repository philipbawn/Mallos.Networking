namespace Mallos.Networking.ServerTest
{
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServicesCollection();
            serviceProvider.AddConfig(args);

            var server = new NetServer(serviceProvider);

            server.Start();

            Helpers.WaitUntilRunning(server);
            while (server.Running)
            {
                Thread.Sleep(100);
            }
        }
    }
}
