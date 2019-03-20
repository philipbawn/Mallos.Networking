namespace Mallos.Networking.User.Abstractions
{
    using System;
    using System.Threading.Tasks;

    public interface IUserStorage<TUser>
        where TUser : IdentityUser
    {
        /// <summary>
        /// Asynchronously inserts a new user.
        /// </summary
        /// <param name="user">The user.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<bool> CreateAsync(TUser user);

        /// <summary>
        /// Asynchronously updates a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>true if the user has been updated; otherwise, false.</returns>
        Task<bool> UpdateAsync(TUser user);

        /// <summary>
        /// Asynchronously deletes a new user.
        /// </summary
        /// <param name="user">The user.</param>
        /// <returns>true if the user has been deleted; otherwise, false.</returns>
        Task<bool> DeleteAsync(TUser user);

        /// <summary>
        /// Asynchronously finds a user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>User if found; otherwise, null.</returns>
        Task<TUser> FindByNameAsync(string username);

        /// <summary>
        /// Asynchronously finds a user by id.
        /// </summary>
        /// <param name="guid">The unique id.</param>
        /// <returns>User if found; otherwise, null.</returns>
        Task<TUser> FindByIdAsync(Guid guid);
    }
}
