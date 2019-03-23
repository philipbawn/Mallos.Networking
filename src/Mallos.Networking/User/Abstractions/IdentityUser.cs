namespace Mallos.Networking.User.Abstractions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class IdentityUser
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
        /// Initialize a new <see cref="IdentityUser"/>.
        /// </summary>
        /// <param name="username">The username.</param>
        public IdentityUser(string username)
        {
            this.Username = username;
        }

        public IdentityUser(SerializationInfo info, StreamingContext context)
        {
            this.Guid = (Guid)info.GetValue("Guid", typeof(Guid));
            this.Username = info.GetString("Username");
            this.PasswordHash = info.GetString("PasswordHash");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Guid", Guid);
            info.AddValue("Username", Username);
            info.AddValue("PasswordHash", PasswordHash);
        }
    }
}
