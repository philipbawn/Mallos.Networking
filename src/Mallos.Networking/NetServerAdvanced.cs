namespace Mallos.Networking
{
    using System;
    using Mallos.Networking.User;

    public class NetServer<TUser> : NetServer
        where TUser : IdentityUser
    {
        public UserManager<TUser> UserManager { get; }

        public NetServer(IServiceProvider serviceProvider, UserManager<TUser> userManager) 
            : base(serviceProvider)
        {
            this.UserManager = userManager;
        }
    }
}
