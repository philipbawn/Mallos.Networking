namespace Mallos.Networking.User
{
    using System.Threading.Tasks;

    public interface IUserManager
    {
        /// <summary>
        /// Asynchronously gets the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        Task<IUser> FindUserByNameAsync(string username);

        /// <summary>
        /// Asynchronously creates a new user.
        /// </summary>
        /// <param name="user">The new user.</param>
        /// <returns>true if the user is created; otherwise, false.</returns>
        Task<bool> CreateNewUserAsync(IUser user);
    }
}
