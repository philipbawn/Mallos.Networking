namespace Mallos.Networking.ServerTest
{
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = Services.Create(args);

            var server = new NetServer(serviceProvider);

            server.Start();

            Helpers.WaitUntilRunning(server);
            while (server.Running)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
