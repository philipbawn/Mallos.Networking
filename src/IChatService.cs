namespace Mallos.Networking
{
    public interface IChatService
    {
        void SendMessage(string message);
        void SendMessage(string channel, string message);
    }
}
