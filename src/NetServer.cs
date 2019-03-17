namespace Mallos.Networking
{
    using System;
    using Mallos.Networking.Handlers;
    using Mallos.Networking.Packets;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Networker.Formatter.ZeroFormatter;
    using Networker.Server;
    using Networker.Server.Abstractions;

    public class NetServer : NetPeer
    {
        public override bool Running => server != null && server.Information.IsRunning;

        private readonly IServer server;

        public NetServer(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            this.server = CreateServer(serviceProvider);
            this.server.ClientConnected += ClientConnected;
            this.server.ClientDisconnected += ClientDisconnected;
            this.server.Start();
        }

        public override void SendMessage(string message) => server.Broadcast(new MessagePacket(null, message));
        public override void SendMessage(string channel, string message) => throw new System.NotImplementedException();

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

        private IServer CreateServer(IServiceProvider serviceProvider)
        {
            var config = (IConfiguration)serviceProvider.GetService(typeof(IConfiguration));

            return new ServerBuilder()
                .UseTcp(config.GetValue<int>("Network:TcpPort"))
                .UseUdp(config.GetValue<int>("Network:UdpPort"))
                .UseZeroFormatter()
                .UseConfiguration(config)
                .ConfigureLogging(builder =>
                {
                    builder.AddConfiguration(config.GetSection("Logging"));
                    // builder.AddConsole();
                })
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
