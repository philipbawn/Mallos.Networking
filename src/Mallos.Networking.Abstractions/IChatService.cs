namespace Mallos.Networking
{
    using System;

    public interface IChatService
    {
        event Action<ChatMessage> Received;

        void SendMessage(string message);
        void SendMessage(string channel, string message);
    }
}
