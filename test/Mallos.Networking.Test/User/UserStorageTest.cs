namespace Mallos.Networking.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class UserStorageTest
    {
        [Theory]
        [MemberData(nameof(UsernameData))]
        public async Task CreateAndGetUser(IdentityUser[] testUsers)
        {
            var storage = new InMemoryUserStorage<IdentityUser>();

            // Create Users
            foreach (var newUser in testUsers)
            {
                Assert.True(await storage.CreateAsync(newUser));
            }

            // Get Users again by username
            foreach (var newUser in testUsers)
            {
                var user = await storage.FindByNameAsync(newUser.Username);
                Assert.Equal(newUser.Username, user.Username);
            }

            // Gets Users again by guid (this works because it's using in-memory storage)
            foreach (var newUser in testUsers)
            {
                var user = await storage.FindByIdAsync(newUser.Guid);
                Assert.Equal(newUser.Username, user.Username);
            }
        }

        public static IEnumerable<object[]> UsernameData =>
            new List<object[]>()
            {
                new object[] {
                    new IdentityUser[] {
                        new IdentityUser("Eric"),
                        new IdentityUser("Julia"),
                        new IdentityUser("Linus")
                    }
                }
            };
    }
}
