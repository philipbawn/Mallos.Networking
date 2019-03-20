namespace Mallos.Networking.Chat.Abstractions
{
    using System;

    public readonly struct ChatSender
    {
        public readonly Guid Id;
        public readonly string Username;

        public ChatSender(Guid id, string username)
        {
            this.Id = id;
            this.Username = username;
        }
    }
}
