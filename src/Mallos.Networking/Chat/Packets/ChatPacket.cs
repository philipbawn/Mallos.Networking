namespace Mallos.Networking.Chat.Packets
{
    using Networker.Formatter.ZeroFormatter;
    using System.ComponentModel;
    using ZeroFormatter;

    /// <summary>
    /// This packet is sent from the client.
    /// </summary>
    [ZeroFormattable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ChatPacket : ZeroFormatterPacketBase
    {
        [Index(2)]
        public virtual string Channel { get; set; }

        [Index(3)]
        public virtual string Message { get; set; }

        public ChatPacket() { }
        public ChatPacket(string channel, string message)
        {
            this.Channel = channel;
            this.Message = message;
        }

        public override string ToString() => $"{Channel}: {Message}";
    }
}
