namespace Mallos.Networking.Chat.Abstractions
{
    using Mallos.Networking.User.Abstractions;
    using System;
    using System.Collections.ObjectModel;

    public interface IChatService<TUser>
        where TUser : IdentityUser
    {
        ObservableCollection<ChatMessage> Messages { get; }

        event Action<ChatMessage> Received;

        void SendMessage(string message);
        void SendMessage(string channel, string message);
    }
}
