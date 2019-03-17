namespace Mallos.Networking.Chat
{
    public readonly struct ChatMessage
    {
        public readonly ChatSender Sender;
        public readonly string Channel;
        public readonly string Message;

        public ChatMessage(ChatSender sender, string channel, string message)
        {
            this.Sender = sender;
            this.Channel = channel;
            this.Message = message;
        }

        public override string ToString() => (Channel != null) ? $"{Channel}: {Message}" : Message;
    }
}
