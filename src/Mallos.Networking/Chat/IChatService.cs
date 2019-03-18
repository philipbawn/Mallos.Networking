namespace Mallos.Networking.Chat
{
    using System;
    using System.Collections.ObjectModel;

    public interface IChatService
    {
        ObservableCollection<ChatMessage> Messages { get; }

        event Action<ChatMessage> Received;

        void SendMessage(string message);
        void SendMessage(string channel, string message);
    }
}
