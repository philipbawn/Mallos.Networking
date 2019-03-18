namespace Mallos.Networking
{
    using Microsoft.Extensions.DependencyInjection;
    using Networker.Server;
    using Networker.Server.Abstractions;
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    public class NetServerCore : NetPeer
    {
        /// <inheritdoc />
        public override NetPeerStatus Status => (NetworkerServer != null && NetworkerServer.Information.IsRunning) ? NetPeerStatus.Online : NetPeerStatus.Offline;

        protected IServer NetworkerServer { get; private set; }

        /// <summary>
        /// Initialize a new <see cref="NetServerCore"/>.
        /// </summary>
        /// <param name="serviceProvider">The services.</param>
        public NetServerCore(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

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
        public override void SendPacket<T>(T packet)
        {
            if (Status == NetPeerStatus.Online)
            {
                // TODO: TCP and handle channel.
                NetworkerServer.Broadcast(packet);
            }
        }

        /// <summary>
        /// Called before building the <see cref="IServer"/>.
        /// </summary>
        /// <param name="builder">The server builder.</param>
        protected virtual void OnServerBuild(IServerBuilder builder)
        {
            // Apply custom server properties.
        }

        internal virtual void RegisterTypes(IServiceCollection serviceCollection)
        {
            // TODO: Waiting for https://github.com/MarkioE/Networker/issues/35
        }

        private void ClientConnected(object sender, TcpConnectionConnectedEventArgs args)
        {

        }

        private void ClientDisconnected(object sender, TcpConnectionDisconnectedEventArgs args)
        {
            
        }
    }
}
