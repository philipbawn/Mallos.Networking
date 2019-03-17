namespace Mallos.Networking.Chat
{
    using System;

    public interface IChatService
    {
        event Action<ChatMessage> Received;

        void SendMessage(string message);
        void SendMessage(string channel, string message);
    }
}
