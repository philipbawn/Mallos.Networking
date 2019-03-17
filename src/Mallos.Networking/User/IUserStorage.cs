namespace Mallos.Networking.User
{
    using System;
    using System.Threading.Tasks;

    public interface IUserStorage
    {
        /// <summary>
        /// Asynchronously inserts a new user.
        /// </summary
        /// <param name="user">The user.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<bool> CreateAsync(User user);

        /// <summary>
        /// Asynchronously updates a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>true if the user has been updated; otherwise, false.</returns>
        Task<bool> UpdateAsync(User user);

        /// <summary>
        /// Asynchronously deletes a new user.
        /// </summary
        /// <param name="user">The user.</param>
        /// <returns>true if the user has been deleted; otherwise, false.</returns>
        Task<bool> DeleteAsync(User user);

        /// <summary>
        /// Asynchronously finds a user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>User if found; otherwise, null.</returns>
        Task<User> FindByNameAsync(string username);

        /// <summary>
        /// Asynchronously finds a user by id.
        /// </summary>
        /// <param name="guid">The unique id.</param>
        /// <returns>User if found; otherwise, null.</returns>
        Task<User> FindByIdAsync(Guid guid);
    }
}
