namespace Mallos.Networking
{
    using Mallos.Networking.Chat;
    using Microsoft.Extensions.Logging;
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    public enum NetPeerStatus
    {
        Offline,
        Connecting,
        Online
    }

    /// <summary>
    /// Peer class for <see cref="NetServer"/> and <see cref="NetClient"/> creating a shared API for both.
    /// </summary>
    public abstract class NetPeer
    {
        /// <summary>
        /// Gets the <see cref="NetPeerStatus"/>.
        /// </summary>
        public abstract NetPeerStatus Status { get; }

        /// <summary>
        /// Gets the connected <see cref="NetConnectionParameters"/>.
        /// </summary>
        public NetConnectionParameters Parameters { get; protected set; }

        /// <summary>
        /// Gets the chat client.
        /// </summary>
        public IChatService Chat { get; protected set; }

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> used by this class.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Gets the <see cref="NetPeer"/>'s logger.
        /// </summary>
        protected ILogger Logger { get; }

        protected NetPeer(IServiceProvider serviceProvider)
        {
            this.Services = serviceProvider;
            this.Chat = new ChatService(this);
            this.Logger = serviceProvider.TryCreateLogger<NetPeer>();
        }

        public abstract Task<bool> Start(NetConnectionParameters parameters = default);

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public abstract void SendPacket<T>(T packet);
    }
}
