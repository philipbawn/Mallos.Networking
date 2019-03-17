namespace Mallos.Networking.ServerTest
{
    using Mallos.Networking.User;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = Services.Create(args);

            var userManager = CreateUserManagerAsync().GetAwaiter().GetResult();
            var server = new NetServer<IdentityUser>(serviceProvider, userManager);

            AddEvents(server);

            server.Start().GetAwaiter().GetResult();

            while (server.Status != NetPeerStatus.Offline)
            {
                Thread.Sleep(1000);
            }
        }

        static void AddEvents(NetServer<IdentityUser> server)
        {
            server.Chat.Received += (message) =>
            {
                Console.WriteLine(message.ToString());
            };
        }

        static async Task<UserManager<IdentityUser>> CreateUserManagerAsync()
        {
            var userStorage = new InMemoryUserStorage<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStorage);

            await userManager.CreateAsync(new IdentityUser("Eric"), "abc123");

            return userManager;
        }
    }
}
