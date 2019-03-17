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
            var userManager =  new UserManager<IdentityUser>(userStorage);

            await userManager.CreateAsync(new IdentityUser("Eric"), "abc123");

            return userManager;
        }

        public static void WaitUntilRunning(NetPeer netPeer)
        {
            int retries = 100;
            while (retries > 0 && netPeer.Status == NetPeerStatus.Connecting)
            {
                Thread.Sleep(10);
                retries -= 1;
            }
        }
    }
}
