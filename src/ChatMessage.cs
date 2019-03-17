namespace Mallos.Networking
{
    public readonly struct ChatMessage
    {
        public readonly string Channel;
        public readonly string Message;

        internal ChatMessage(string channel, string message)
        {
            this.Channel = channel;
            this.Message = message;
        }

        public override string ToString() => (Channel != null) ? $"{Channel}: {Message}" : Message;
    }
}
