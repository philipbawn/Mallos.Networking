namespace Mallos.Networking
{
    using Mallos.Networking.Chat;
    using Mallos.Networking.User;
    using Mallos.Networking.User.Abstractions;
    using Microsoft.Extensions.DependencyInjection;
    using Networker.Server;
    using Networker.Server.Abstractions;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading.Tasks;

    public class NetServer<TUser> : NetPeer<TUser>
        where TUser : IdentityUser
    {
        /// <inheritdoc />
        public override NetPeerStatus Status => (NetworkerServer != null && NetworkerServer.Information.IsRunning) ? NetPeerStatus.Online : NetPeerStatus.Offline;

        /// <summary>
        /// Gets the <see cref="UserManager"/>.
        /// </summary>
        public UserManager<TUser> UserManager { get; }

        /// <summary>
        /// Gets all the currently connected <see cref="IdentityUser"/>'s.
        /// </summary>
        public ObservableCollection<TUser> ConnectedUsers { get; }

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
            this.ConnectedUsers = new ObservableCollection<TUser>();
        }

        /// <inheritdoc />
        public override Task<bool> Start(NetConnectionParameters parameters = default)
        {
            var builder = new ServerBuilder().AddDefaultSettings(parameters, this);

            OnServerBuild(builder);

            this.NetworkerServer = builder.Build();
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

        protected internal virtual void OnClientConnected(TUser user)
        {
            ConnectedUsers.Add(user);
        }

        protected internal virtual void OnClientDisconnected(TUser user)
        {
            ConnectedUsers.Remove(user);
        }

        /// <summary>
        /// Called before building the <see cref="IServer"/>.
        /// </summary>
        /// <param name="builder">The server builder.</param>
        protected virtual void OnServerBuild(IServerBuilder builder)
        {
            builder
                .RegisterPacketHandler<User.Packets.LoginPacket, LoginServerHandler<TUser>>()
                .RegisterPacketHandler<Chat.Packets.ChatPacket, ChatServerHandler<TUser>>();
        }

        private void ClientDisconnected(object sender, TcpConnectionDisconnectedEventArgs args)
        {
            OnClientDisconnected((TUser)args.Connection.UserTag);
        }

        internal override void RegisterTypes(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(c => UserManager);
            base.RegisterTypes(serviceCollection);
        }
    }
}
