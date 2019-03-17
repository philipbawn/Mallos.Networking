namespace Mallos.Networking
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Networker.Client.Abstractions;
    using Networker.Formatter.ZeroFormatter;
    using Networker.Server.Abstractions;

    static class Configuration
    {
        public static IServerBuilder AddDefaultSettings(this IServerBuilder builder, NetConnectionParameters parameters, NetPeer peer)
        {
            var config = (IConfiguration)peer.Services.GetService(typeof(IConfiguration));

            var tcpPort = (parameters.TcpPort > 0) ? parameters.TcpPort : config.GetValue<int>("Network:TcpPort");
            var udpPort = (parameters.UdpPort > 0) ? parameters.UdpPort : config.GetValue<int>("Network:UdpPort");

            return builder
                .UseTcp(tcpPort)
                .UseUdp(udpPort)
                .UseZeroFormatter()
                .UseConfiguration(config)
                .ConfigureLogging(logBuilder =>
                {
                    logBuilder.AddConfiguration(config.GetSection("Logging"));
                    logBuilder.AddConsole();
                });
        }

        public static IClientBuilder AddDefaultSettings(this IClientBuilder builder, NetConnectionParameters parameters, NetPeer peer)
        {
            var config = (IConfiguration)peer.Services.GetService(typeof(IConfiguration));

            var tcpPort = (parameters.TcpPort > 0) ? parameters.TcpPort : config.GetValue<int>("Network:TcpPort");
            var udpPort = (parameters.UdpPort > 0) ? parameters.UdpPort : config.GetValue<int>("Network:UdpPort");
            var udpLocalPort = (parameters.UdpLocalPort > 0) ? parameters.UdpLocalPort : config.GetValue<int>("Network:UdpLocalPort");

            return builder
                .UseIp(parameters.Address)
                .UseTcp(tcpPort)
                .UseUdp(udpPort, udpLocalPort)
                .UseZeroFormatter()
                .UseConfiguration(config)
                .ConfigureLogging(logBuilder =>
                {
                    logBuilder.AddConfiguration(config.GetSection("Logging"));
                    logBuilder.AddConsole();
                });
        }
    }
}
