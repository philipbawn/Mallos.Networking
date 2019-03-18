namespace Mallos.Networking
{
    using Mallos.Networking.User;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Networker.Client;
    using Networker.Client.Abstractions;
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    public class NetClient : NetPeer
    {
        /// <inheritdoc />
        public override NetPeerStatus Status => status;

        protected IClient NetworkerClient { get; private set; }

        internal NetPeerStatus status = NetPeerStatus.Offline;

        /// <summary>
        /// Initialize a new <see cref="NetClient"/>.
        /// </summary>
        /// <param name="serviceProvider">The services.</param>
        public NetClient(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        /// <inheritdoc />
        public override Task<bool> Start(NetConnectionParameters parameters = default)
        {
            this.status = NetPeerStatus.Connecting;
            this.Parameters = parameters;

            var builder = new ClientBuilder().AddDefaultSettings(parameters, this);

            OnClientBuild(builder);

            this.NetworkerClient = builder.Build();
            this.NetworkerClient.Connected += ClientConnected;
            this.NetworkerClient.Disconnected += ClientDisconnected;

            var connectResult = this.NetworkerClient.Connect();
            if (!connectResult.Success)
            {
                var errors = string.Join(", ", connectResult.Errors.ToArray());
                Logger?.LogWarning(errors);
                return Task.FromResult(false);
            }

            NetworkerClient.Send(new LoginPacket(parameters.Username, parameters.Password));

            var config = (IConfiguration)Services.GetService(typeof(IConfiguration));
            var timeout = config.GetValue<int>("Network:ConnectTimeout");
            if (timeout <= 2) timeout = 2;

            int timeoutSeconds = timeout * 2;
            while (timeoutSeconds > 0 && (status == NetPeerStatus.Connecting || 
                                          status == NetPeerStatus.Authenticating))
            {
                Thread.Sleep(500);
                timeoutSeconds -= 1;
            }

            return Task.FromResult(status == NetPeerStatus.Online);
        }

        /// <inheritdoc />
        public override Task Stop()
        {
            this.status = NetPeerStatus.Offline;
            this.NetworkerClient.Stop();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SendPacket<T>(T packet)
        {
            if (Status == NetPeerStatus.Online)
            {
                NetworkerClient.Send(packet);
            }
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
            this.status = NetPeerStatus.Authenticating;
        }

        private void ClientDisconnected(object sender, Socket args)
        {
            this.status = NetPeerStatus.Offline;
        }
    }
}
