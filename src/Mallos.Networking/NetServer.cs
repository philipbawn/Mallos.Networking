namespace Mallos.Networking
{
    using Mallos.Networking.User;
    using Networker.Server;
    using Networker.Server.Abstractions;
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    public class NetServer : NetPeer
    {
        public override bool Running => server != null && server.Information.IsRunning;

        private IServer server;

        public NetServer(IServiceProvider serviceProvider, IUserManager userManager)
            : base(serviceProvider)
        {

        }

        public override Task Start(NetConnectionParameters parameters = default)
        {
            this.Parameters = parameters;

            this.server = CreateServer();
            this.server.ClientConnected += ClientConnected;
            this.server.ClientDisconnected += ClientDisconnected;

            this.server.Start();

            return Task.CompletedTask;
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
                .Build();

            // .RegisterPacketHandler<PlayerUpdatePacket, PlayerUpdatePacketHandler>()
        }
    }
}
