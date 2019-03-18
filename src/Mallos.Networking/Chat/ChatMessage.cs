namespace Mallos.Networking.Chat
{
    using Mallos.Networking.User;

    public readonly struct ChatMessage
    {
        public readonly IdentityUser Sender;
        public readonly string Channel;
        public readonly string Message;

        public ChatMessage(IdentityUser sender, string channel, string message)
        {
            this.Sender = sender;
            this.Channel = channel;
            this.Message = message;
        }

        public override string ToString()
        {
            var username = Sender?.Username ?? "Unknown";

            if (string.IsNullOrEmpty(Channel))
            {
                return $"{username}: {Message}";
            }
            else
            {
                return $"[{Channel}] {username}: {Message}";
            }
        }
    }
}
