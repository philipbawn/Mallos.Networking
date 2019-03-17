namespace Mallos.Networking
{
    using System;
    using Mallos.Networking.Packets;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Networker.Client;
    using Networker.Client.Abstractions;
    using Networker.Formatter.ZeroFormatter;
    using System.Net.Sockets;

    public class NetClient : NetPeer
    {
        public override bool Running => isRunning;

        public readonly NetConnectionParameters Parameters;

        private readonly IClient client;

        private bool isRunning = false;

        public NetClient(IServiceProvider serviceProvider, NetConnectionParameters parameters)
            : base(serviceProvider)
        {
            this.Parameters = parameters;

            this.client = CreateClient(serviceProvider, parameters);
            this.client.Connected += ClientConnected;
            this.client.Disconnected += ClientDisconnected;
            this.client.Connect();
        }

        public override void SendMessage(string message) => client.Send(new MessagePacket(null, message));
        public override void SendMessage(string channel, string message) => client.Send(new MessagePacket(channel, message));

        private void ClientConnected(object sender, Socket args)
        {
            this.isRunning = true;
        }

        private void ClientDisconnected(object sender, Socket args)
        {
            this.isRunning = false;
        }

        private IClient CreateClient(IServiceProvider serviceProvider, NetConnectionParameters parameters)
        {
            var config = (IConfiguration)serviceProvider.GetService(typeof(IConfiguration));

            var tcpPort = (parameters.TcpPort > 0) ? parameters.TcpPort : config.GetValue<int>("Network:TcpPort");
            var udpPort = (parameters.UdpPort > 0) ? parameters.UdpPort : config.GetValue<int>("Network:UdpPort");
            var udpLocalPort = (parameters.UdpLocalPort > 0) ? parameters.UdpLocalPort : config.GetValue<int>("Network:UdpLocalPort");

            return new ClientBuilder()
                .UseIp(parameters.Address)
                .UseTcp(tcpPort)
                .UseUdp(udpPort, udpLocalPort)
                .UseZeroFormatter()
                .UseConfiguration(config)
                .ConfigureLogging(builder =>
                {
                    builder.AddConfiguration(config.GetSection("Logging"));
                    //builder.AddConsole();
                })
                .RegisterTypes(serviceCollection =>
                {
                  
                })
                .Build();
        }
    }
}
