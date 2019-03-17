namespace Mallos.Networking
{
    using Networker.Client;
    using Networker.Client.Abstractions;
    using System;
    using System.ComponentModel;
    using System.Net.Sockets;

    public class NetClient : NetPeer
    {
        public override bool Running => isRunning;

        private IClient client;
        private bool isRunning = false;

        public NetClient(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        public override void Start(NetConnectionParameters parameters = default, Action<NetPeer, NetPeerStatus> callback = null)
        {
            this.Parameters = parameters;

            this.client = CreateClient(parameters);
            this.client.Connected += ClientConnected;
            this.client.Disconnected += ClientDisconnected;
            this.client.Connect();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SendPacket<T>(T packet)
        {
            client.Send(packet);
        }

        private void ClientConnected(object sender, Socket args)
        {
            this.isRunning = true;
        }

        private void ClientDisconnected(object sender, Socket args)
        {
            this.isRunning = false;
        }

        private IClient CreateClient(NetConnectionParameters parameters)
        {
            return new ClientBuilder()
                .AddDefaultSettings(parameters, this)
                .RegisterTypes(serviceCollection =>
                {

                })
                .Build();
        }
    }
}
