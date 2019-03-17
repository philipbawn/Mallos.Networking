namespace Mallos.Networking.Chat
{
    using Mallos.Networking.Packets;
    using Microsoft.Extensions.Logging;

    class ChatService : IChatService
    {
        private readonly NetPeer netPeer;
        private readonly ILogger logger;

        public ChatService(NetPeer netPeer)
        {
            this.netPeer = netPeer;

            var loggerFactory = (ILoggerFactory)netPeer.Services.GetService(typeof(ILoggerFactory));
            if (loggerFactory != null)
            {
                this.logger = loggerFactory.CreateLogger<ChatService>();
            }
        }

        public void SendMessage(string message)
        {
            netPeer.SendPacket(new MessagePacket(null, message));
            logger?.LogInformation("Send: {message}", message);
        }

        public void SendMessage(string channel, string message)
        {
            netPeer.SendPacket(new MessagePacket(channel, message));
            logger?.LogInformation("Send: [{channel}] {message}", channel, message);
        }
    }
}
