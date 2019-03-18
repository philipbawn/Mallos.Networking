namespace Mallos.Networking
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Mallos.Networking.Chat;
    using Mallos.Networking.User;
    using Microsoft.Extensions.DependencyInjection;
    using Networker.Server;
    using Networker.Server.Abstractions;

    public class NetServer<TUser> : NetPeer
        where TUser : IdentityUser
    {
        /// <inheritdoc />
        public override NetPeerStatus Status => (NetworkerServer != null && NetworkerServer.Information.IsRunning) ? NetPeerStatus.Online : NetPeerStatus.Offline;

        /// <summary>
        /// Gets the <see cref="UserManager"/>.
        /// </summary>
        public UserManager<TUser> UserManager { get; }

        protected IServer NetworkerServer { get; private set; }

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

        /// <inheritdoc />
        public override Task<bool> Start(NetConnectionParameters parameters = default)
        {
            var builder = new ServerBuilder().AddDefaultSettings(parameters, this);

            OnServerBuild(builder);

            this.NetworkerServer = builder.Build();
            this.NetworkerServer.ClientConnected += ClientConnected;
            this.NetworkerServer.ClientDisconnected += ClientDisconnected;

            this.NetworkerServer.Start();

            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public override Task Stop()
        {
            this.NetworkerServer.Stop();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SendPacket<T>(T packet, NetPeerProtocol protocol = NetPeerProtocol.Tcp)
        {
            if (Status == NetPeerStatus.Online)
            {
                if (protocol == NetPeerProtocol.Tcp)
                {
                    NetworkerServer.GetConnections().Broadcast(packet);
                }
                else
                {
                    NetworkerServer.Broadcast(packet);
                }
            }
        }

        /// <summary>
        /// Called before building the <see cref="IServer"/>.
        /// </summary>
        /// <param name="builder">The server builder.</param>
        protected virtual void OnServerBuild(IServerBuilder builder)
        {
            builder
                .RegisterPacketHandler<LoginPacket, LoginPacketHandler<TUser>>()
                .RegisterPacketHandler<ChatPacket, ChatPacketHandler<TUser>>();
        }

        internal override void RegisterTypes(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(c => UserManager);
            base.RegisterTypes(serviceCollection);
        }

        private void ClientConnected(object sender, TcpConnectionConnectedEventArgs args)
        {

        }

        private void ClientDisconnected(object sender, TcpConnectionDisconnectedEventArgs args)
        {

        }
    }
}
