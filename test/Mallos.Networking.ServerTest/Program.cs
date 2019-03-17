namespace Mallos.Networking.ServerTest
{
    using System;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = Services.Create(args);

            var userManager = Helpers.CreateUserManagerAsync().GetAwaiter().GetResult();
            var server = new NetServer(serviceProvider, userManager);

            server.Chat.Received += (message) =>
            {
                Console.WriteLine(message.ToString());
            };

            server.Start();

            Helpers.WaitUntilRunning(server);
            while (server.Running)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
