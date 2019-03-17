namespace Mallos.Networking.User
{
    using System;

    public class User
    {
        /// <summary>
        /// Gets a unique id assigned to the user.
        /// </summary>
        public Guid Guid { get; internal set; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Gets the hashed password.
        /// </summary>
        public string PasswordHash { get; internal set; }

        /// <summary>
        /// Initialize a new <see cref="User"/>.
        /// </summary>
        /// <param name="username">The username.</param>
        public User(string username)
        {
            this.Username = username;
        }
    }
}
