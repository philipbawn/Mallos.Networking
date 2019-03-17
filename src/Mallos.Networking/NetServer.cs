namespace Mallos.Networking
{
    using Networker.Server;
    using Networker.Server.Abstractions;
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    public class NetServer : NetPeer
    {
        /// <inheritdoc />
        public override NetPeerStatus Status => (NetworkerServer != null && NetworkerServer.Information.IsRunning) ? NetPeerStatus.Online : NetPeerStatus.Offline;

        protected IServer NetworkerServer { get; private set; }

        /// <summary>
        /// Initialize a new <see cref="NetServer"/>.
        /// </summary>
        /// <param name="serviceProvider">The services.</param>
        public NetServer(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        /// <inheritdoc />
        public override Task Start(NetConnectionParameters parameters = default)
        {
            this.Parameters = parameters;

            var builder = new ServerBuilder().AddDefaultSettings(parameters, this);

            OnServerBuild(builder);

            this.NetworkerServer = builder.Build();
            this.NetworkerServer.ClientConnected += ClientConnected;
            this.NetworkerServer.ClientDisconnected += ClientDisconnected;

            this.NetworkerServer.Start();

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SendPacket<T>(T packet)
        {
            // TODO: TCP and handle channel.
            NetworkerServer.Broadcast(packet);
        }

        /// <summary>
        /// Called before building the <see cref="IServer"/>.
        /// </summary>
        /// <param name="builder">The server builder.</param>
        protected virtual void OnServerBuild(IServerBuilder builder)
        {
            // Apply custom server properties.
        }

        private void ClientConnected(object sender, TcpConnectionConnectedEventArgs args)
        {
            System.Console.WriteLine($"Client Connected to server {args.Connection.Socket.RemoteEndPoint}");

            // TODO: Sync GameSession with the client.
            // IPacketSerialiser packetSerialiser = null;
            // args.Connection.Socket.Send(packetSerialiser.Serialise(packet));
        }

        private void ClientDisconnected(object sender, TcpConnectionDisconnectedEventArgs args)
        {
            System.Console.WriteLine($"Client Disconnected from server {args.Connection.Socket.RemoteEndPoint}");
        }
    }
}
