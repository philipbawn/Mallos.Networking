namespace Mallos.Networking
{
    using System;
    using Mallos.Networking.User;

    public class NetServer<TUser> : NetServerCore
        where TUser : IdentityUser
    {
        /// <summary>
        /// Gets the <see cref="UserManager"/>.
        /// </summary>
        public UserManager<TUser> UserManager { get; }

        /// <summary>
        /// Initialize a new <see cref="NetServer{TUser}"/>.
        /// </summary>
        /// <param name="serviceProvider">The services.</param>
        /// <param name="userManager">The user manager.</param>
        public NetServer(IServiceProvider serviceProvider, UserManager<TUser> userManager) 
            : base(serviceProvider)
        {
            this.UserManager = userManager;
        }
    }
}
