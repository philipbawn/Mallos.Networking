namespace Mallos.Networking
{
    using Networker.Client;
    using Networker.Client.Abstractions;
    using System;
    using System.ComponentModel;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class NetClient : NetPeer
    {
        /// <inheritdoc />
        public override NetPeerStatus Status => status;

        protected IClient NetworkerClient { get; private set; }

        private NetPeerStatus status = NetPeerStatus.Offline;

        /// <summary>
        /// Initialize a new <see cref="NetClient"/>.
        /// </summary>
        /// <param name="serviceProvider">The services.</param>
        public NetClient(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        /// <inheritdoc />
        public override Task Start(NetConnectionParameters parameters = default)
        {
            this.status = NetPeerStatus.Connecting;
            this.Parameters = parameters;

            var builder = new ClientBuilder().AddDefaultSettings(parameters, this);

            OnClientBuild(builder);

            this.NetworkerClient = builder.Build();
            this.NetworkerClient.Connected += ClientConnected;
            this.NetworkerClient.Disconnected += ClientDisconnected;
            this.NetworkerClient.Connect();

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SendPacket<T>(T packet)
        {
            NetworkerClient.Send(packet);
        }

        /// <summary>
        /// Called before building the <see cref="IClient"/>.
        /// </summary>
        /// <param name="builder">The client builder.</param>
        protected virtual void OnClientBuild(IClientBuilder builder)
        {
            // Apply custom client properties.
        }

        private void ClientConnected(object sender, Socket args)
        {
            this.status = NetPeerStatus.Online;
        }

        private void ClientDisconnected(object sender, Socket args)
        {
            this.status = NetPeerStatus.Offline;
        }
    }
}
