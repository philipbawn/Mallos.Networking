namespace Mallos.Networking.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class UserManagerTest
    {
        [Theory]
        [MemberData(nameof(UserData))]
        public async Task CreateUserAndLogin(User user, string password, bool expectCreateSuccess)
        {
            var userManager = await CreateUserManagerAsync();

            var createdResult = await userManager.CreateAsync(user, password);
            Assert.Equal(expectCreateSuccess, createdResult.Success);

            if (expectCreateSuccess)
            {
                var loginResult = await userManager.AddLoginAsync(user.Username, password);
                Assert.True(loginResult.Success);
            }
        }

        public static async Task<UserManager> CreateUserManagerAsync()
        {
            var userStorage = new InMemoryUserStorage();
            var userManager = new UserManager(userStorage);

            await userManager.CreateAsync(new User("Eric"), "abc123");

            return userManager;
        }

        public static IEnumerable<object[]> UserData =>
            new List<object[]>()
            {
                new object[] { new User("Eric"), "abc123", false },
                new object[] { new User("Julia"), "123abc", true },
                new object[] { new User("Linus"), "password", true }
            };
    }
}
