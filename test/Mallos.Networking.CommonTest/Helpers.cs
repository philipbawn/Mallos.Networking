namespace Mallos.Networking
{
    using Mallos.Networking.User;
    using System.Threading;
    using System.Threading.Tasks;

    static class Helpers
    {
        public static async Task<UserManager<IdentityUser>> CreateUserManagerAsync()
        {
            var userStorage = new InMemoryUserStorage<IdentityUser>();

            await userStorage.CreateAsync(new IdentityUser("Eric"));

            return new UserManager<IdentityUser>(userStorage);
        }

        public static void WaitUntilRunning(NetPeer netPeer)
        {
            int retries = 100;
            while (retries > 0 && !netPeer.Running)
            {
                Thread.Sleep(10);
                retries -= 1;
            }
        }
    }
}
