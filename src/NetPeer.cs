namespace Mallos.Networking
{
    using System;
    using Networker.Common.Abstractions;

    public abstract class NetPeer // : INetPeer
    {
        public abstract bool Running { get; }

        public readonly IServiceProvider Services;

        protected NetPeer(IServiceProvider serviceProvider)
        {
            this.Services = serviceProvider;
        }

        public abstract void SendMessage(string message);
        public abstract void SendMessage(string channel, string message);

        // private T AddDefaultSettings<T>(T builder)
        //     where T : IBuilder
        // {
        //     return builder
        //         .UseZeroFormatter()
        //         .UseConfiguration(GameSession.Configuration);
        // }
    }
}
