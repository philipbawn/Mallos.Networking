namespace Mallos.Networking.User
{
    using System;
    using System.Threading.Tasks;

    public interface IUser
    {
        /// <summary>
        /// Gets a unique id assigned to the user.
        /// </summary>
        Guid UniqueId { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Asynchronously sets the username.
        /// </summary>
        /// <param name="username">The new username.</param>
        /// <returns>true if the user has a username set; otherwise, false.</returns>
        Task<bool> SetUsernameAsync(string username);

        /// <summary>
        /// Indicates whether the user has a password set.
        /// </summary>
        /// <returns>true if the user has a password set; otherwise, false.</returns>
        Task<bool> HasPasswordAsync();

        /// <summary>
        /// Asynchronously gets the user password hash.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<string> GetPasswordHashAsync();

        /// <summary>
        /// Asynchronously sets the user password hash.
        /// </summary>
        /// <param name="passwordHash">The password hash.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task SetPasswordHashAsync(string passwordHash);
    }
}
