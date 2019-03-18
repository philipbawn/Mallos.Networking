namespace Mallos.Networking.Chat
{
    using System;
    using System.Collections.ObjectModel;

    class ChatService : IChatService
    {
        public ObservableCollection<ChatMessage> Messages { get; }

        public event Action<ChatMessage> Received;

        private readonly NetPeer netPeer;

        public ChatService(NetPeer netPeer)
        {
            this.netPeer = netPeer;
            this.Messages = new ObservableCollection<ChatMessage>();
        }

        public void SendMessage(string message)
        {
            netPeer.SendPacket(new ChatPacket(null, message));
        }

        public void SendMessage(string channel, string message)
        {
            netPeer.SendPacket(new ChatPacket(channel, message));
        }

        internal void InvokeReceived(ChatMessage message)
        {
            Messages.Add(message);
            Received?.Invoke(message);
        }
    }
}
