namespace Mallos.Networking.Chat.Packets
{
    using Networker.Formatter.ZeroFormatter;
    using System;
    using System.ComponentModel;
    using ZeroFormatter;

    /// <summary>
    /// This packet is sent from the server.
    /// </summary>
    [ZeroFormattable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ChatReplyPacket : ZeroFormatterPacketBase
    {
        [Index(2)]
        public virtual Guid Sender { get; set; }

        [Index(3)]
        public virtual string Message { get; set; }

        public ChatReplyPacket() { }
        public ChatReplyPacket(Guid sender, string message)
        {
            this.Sender = sender;
            this.Message = message;
        }

        public override string ToString() => $"{Sender}: {Message}";
    }
}
