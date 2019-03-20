namespace Mallos.Networking
{
    using Mallos.Networking.Chat;
    using Mallos.Networking.Chat.Abstractions;
    using Mallos.Networking.User.Abstractions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Networker.Client.Abstractions;
    using Networker.Formatter.ZeroFormatter;
    using Networker.Server.Abstractions;
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    public enum NetPeerStatus
    {
        /// <summary>
        /// Network is offline.
        /// </summary>
        Offline,

        /// <summary>
        /// Network is connecting.
        /// </summary>
        Connecting,

        /// <summary>
        /// Network is waiting for authentication.
        /// </summary>
        Authenticating,

        /// <summary>
        /// Network is online and authenticated.
        /// </summary>
        Online
    }

    public enum NetPeerProtocol
    {
        Tcp,
        udp
    }

    /// <summary>
    /// Peer class for <see cref="NetServer"/> and <see cref="NetClient"/> creating a shared API for both.
    /// </summary>
    public abstract class NetPeer<TUser>
        where TUser : IdentityUser
    {
        /// <summary>
        /// Gets the <see cref="NetPeerStatus"/>.
        /// </summary>
        public abstract NetPeerStatus Status { get; }

        /// <summary>
        /// Gets the chat client.
        /// </summary>
        public IChatService<TUser> Chat { get; protected set; }

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> used by this class.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Gets the <see cref="NetPeer"/>'s logger.
        /// </summary>
        protected internal ILogger Logger { get; }

        protected NetPeer(IServiceProvider serviceProvider)
        {
            this.Services = serviceProvider;
            this.Chat = new ChatService<TUser>(this);
            this.Logger = serviceProvider.TryCreateLogger<NetPeer<TUser>>();
        }

        public abstract Task<bool> Start(NetConnectionParameters parameters = default);

        public abstract Task Stop();

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public abstract void SendPacket<T>(T packet, NetPeerProtocol protocol = NetPeerProtocol.Tcp);

        protected virtual void RegisterTypes(IServiceCollection serviceCollection)
        {

        }

        protected IServerBuilder AddDefaultSettings(IServerBuilder builder, NetConnectionParameters parameters)
        {
            var config = (IConfiguration)Services.GetService(typeof(IConfiguration));

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
                })
                .RegisterTypes(serviceCollection =>
                {
                    serviceCollection.AddSingleton(c => this);
                    serviceCollection.AddSingleton(c => this.Chat);
                    this.RegisterTypes(serviceCollection);
                });
        }

        protected IClientBuilder AddDefaultSettings(IClientBuilder builder, NetConnectionParameters parameters)
        {
            var config = (IConfiguration)Services.GetService(typeof(IConfiguration));

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
                })
                .RegisterTypes(serviceCollection =>
                {
                    serviceCollection.AddSingleton(c => this);
                    serviceCollection.AddSingleton(c => this.Chat);
                    this.RegisterTypes(serviceCollection);
                })
                .RegisterPacketHandler<User.Packets.LoginReplyPacket, User.LoginClientHandler>()
                .RegisterPacketHandler<Chat.Packets.ChatReplyPacket, ChatReceiverHandler<TUser>>();
        }
    }
}
