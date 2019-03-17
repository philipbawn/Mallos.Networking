namespace Mallos.Networking.ServerTest
{
    using Mallos.Networking.User;
    using System;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = Services.Create(args);

            var userManager = Helpers.CreateUserManagerAsync().GetAwaiter().GetResult();
            var server = new NetServer<IdentityUser>(serviceProvider, userManager);

            server.Chat.Received += (message) =>
            {
                Console.WriteLine(message.ToString());
            };

            server.Start();

            Helpers.WaitUntilRunning(server);
            while (server.Status != NetPeerStatus.Offline)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
