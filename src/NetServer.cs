namespace Mallos.Networking
{
    using Mallos.Networking.Handlers;
    using Mallos.Networking.Packets;
    using Networker.Server;
    using Networker.Server.Abstractions;
    using System;
    using System.ComponentModel;

    public class NetServer : NetPeer
    {
        public override bool Running => server != null && server.Information.IsRunning;

        private IServer server;

        public NetServer(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        public override void Start(NetConnectionParameters parameters = default, Action<NetPeer, NetPeerStatus> callback = null)
        {
            this.Parameters = parameters;

            this.server = CreateServer();
            this.server.ClientConnected += ClientConnected;
            this.server.ClientDisconnected += ClientDisconnected;

            this.server.Start();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SendPacket<T>(T packet)
        {
            // TODO: TCP and handle channel.
            server.Broadcast(packet);
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

        private IServer CreateServer()
        {
            var parameters = new NetConnectionParameters();
            return new ServerBuilder()
                .AddDefaultSettings(parameters, this)
                .RegisterTypes(serviceCollection =>
                {

                })
                .RegisterPacketHandlerModule<DefaultPacketHandlerModule>()
                .RegisterPacketHandler<MessagePacket, MessageHandler>()
                .Build();

            // .RegisterPacketHandler<PlayerUpdatePacket, PlayerUpdatePacketHandler>()
        }
    }
}
