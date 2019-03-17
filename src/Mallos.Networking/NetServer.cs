namespace Mallos.Networking
{
    using System;
    using Mallos.Networking.User;
    using Networker.Server.Abstractions;

    public class NetServer : NetServerCore
    {
        /// <summary>
        /// Gets the <see cref="UserManager"/>.
        /// </summary>
        public UserManager UserManager { get; }

        /// <summary>
        /// Initialize a new <see cref="NetServer"/>.
        /// </summary>
        /// <param name="serviceProvider">The services.</param>
        /// <param name="userManager">The user manager.</param>
        public NetServer(IServiceProvider serviceProvider, UserManager userManager) 
            : base(serviceProvider)
        {
            this.UserManager = userManager;
        }

        protected override void OnServerBuild(IServerBuilder builder)
        {
            builder.RegisterPacketHandler<LoginPacket, LoginPacketHandler>();
        }
    }
}
