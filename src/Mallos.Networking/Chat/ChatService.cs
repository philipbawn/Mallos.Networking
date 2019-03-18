namespace Mallos.Networking.Chat
{
    using System;
    using Microsoft.Extensions.Logging;

    class ChatService : IChatService
    {
        private readonly NetPeer netPeer;

        public event Action<ChatMessage> Received;

        public ChatService(NetPeer netPeer)
        {
            this.netPeer = netPeer;
        }

        public void SendMessage(string message)
        {
            netPeer.SendPacket(new ChatPacket(null, message));
        }

        public void SendMessage(string channel, string message)
        {
            netPeer.SendPacket(new ChatPacket(channel, message));
        }

        internal void InvokeReceived(ChatMessage message) => Received?.Invoke(message);
    }
}
