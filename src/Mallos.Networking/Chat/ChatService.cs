namespace Mallos.Networking.Chat
{
    using Mallos.Networking.Chat.Abstractions;
    using Mallos.Networking.Chat.Packets;
    using Mallos.Networking.User.Abstractions;
    using System;
    using System.Collections.ObjectModel;

    class ChatService<TUser> : IChatService<TUser>
        where TUser : IdentityUser
    {
        public ObservableCollection<ChatMessage> Messages { get; }

        public event Action<ChatMessage> Received;

        private readonly NetPeer<TUser> netPeer;

        public ChatService(NetPeer<TUser> netPeer)
        {
            this.netPeer = netPeer;
            this.Messages = new ObservableCollection<ChatMessage>();
        }

        public void SendMessage(string message) => SendMessage(string.Empty, message);
        public void SendMessage(string channel, string message)
        {
            if (netPeer is NetClient)
            {
                netPeer.SendPacket(new ChatPacket(channel, message));
            }
            else
            {
                // TODO: Send from Server
                netPeer.SendPacket(new ChatReplyPacket(Guid.NewGuid(), message));
            }
        }

        internal void InvokeReceived(ChatMessage message)
        {
            Messages.Add(message);
            Received?.Invoke(message);
        }
    }
}
