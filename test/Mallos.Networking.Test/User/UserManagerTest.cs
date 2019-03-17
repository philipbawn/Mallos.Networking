namespace Mallos.Networking.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class UserManagerTest
    {
        [Theory]
        [MemberData(nameof(UserData))]
        public async Task CreateUserAndLogin(IdentityUser user, string password, bool expectCreateSuccess)
        {
            var userManager = await Helpers.CreateUserManagerAsync();

            var createdResult = await userManager.CreateAsync(user, password);
            Assert.Equal(expectCreateSuccess, createdResult.Success);

            if (expectCreateSuccess)
            {
                var loginResult = await userManager.AddLoginAsync(user.Username, password);
                Assert.True(loginResult.Success);
            }
        }

        public static IEnumerable<object[]> UserData =>
            new List<object[]>()
            {
                new object[] { new IdentityUser("Eric"), "abc123", false },
                new object[] { new IdentityUser("Julia"), "123abc", true },
                new object[] { new IdentityUser("Linus"), "password", true }
            };
    }
}
