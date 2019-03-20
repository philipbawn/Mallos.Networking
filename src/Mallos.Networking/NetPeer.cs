﻿namespace Mallos.Networking
{
    using Mallos.Networking.Chat;
    using Mallos.Networking.Chat.Abstractions;
    using Mallos.Networking.User.Abstractions;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
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

        internal virtual void RegisterTypes(IServiceCollection serviceCollection)
        {
            // TODO: Waiting for https://github.com/MarkioE/Networker/issues/35
        }
    }
}
