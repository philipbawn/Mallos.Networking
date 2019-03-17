namespace Mallos.Networking.Chat
{
    using System;
    using Microsoft.Extensions.Logging;

    class ChatService : IChatService
    {
        private readonly NetPeer netPeer;
        private readonly ILogger logger;

        public event Action<ChatMessage> Received;

        public ChatService(NetPeer netPeer)
        {
            this.netPeer = netPeer;
            this.logger = netPeer.Services.TryCreateLogger<ChatService>();
        }

        public void SendMessage(string message)
        {
            netPeer.SendPacket(new ChatPacket(null, message));
            logger?.LogInformation("Send: {message}", message);
        }

        public void SendMessage(string channel, string message)
        {
            netPeer.SendPacket(new ChatPacket(channel, message));
            logger?.LogInformation("Send: [{channel}] {message}", channel, message);
        }

        internal void InvokeReceived(ChatMessage message) => Received?.Invoke(message);
    }
}
