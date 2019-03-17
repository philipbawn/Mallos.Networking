namespace Mallos.Networking
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Networker.Common.Abstractions;
    using Networker.Formatter.ZeroFormatter;
    using Networker.Server.Abstractions;

    public enum NetPeerStatus
    {
        Offline,
        Connecting,
        Online
    }

    public abstract class NetPeer
    {
        public abstract bool Running { get; }

        public NetConnectionParameters Parameters { get; protected set; }

        public readonly IServiceProvider Services;

        protected NetPeer(IServiceProvider serviceProvider)
        {
            this.Services = serviceProvider;
        }

        public abstract void Start(NetConnectionParameters parameters = default, Action<NetPeer, NetPeerStatus> callback = null);

        public abstract void SendMessage(string message);
        public abstract void SendMessage(string channel, string message);
    }
}
