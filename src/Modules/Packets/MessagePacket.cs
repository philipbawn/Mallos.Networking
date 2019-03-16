namespace Mallos.Networking.Packets
{
    using Networker.Formatter.ZeroFormatter;
    using ZeroFormatter;

    [ZeroFormattable]
    public class MessagePacket : ZeroFormatterPacketBase
    {
        [Index(2)]
        public virtual string Channel { get; set; }

        [Index(3)]
        public virtual string Message { get; set; }

        public MessagePacket() { }
        public MessagePacket(string channel, string message)
        {
            this.Channel = channel;
            this.Message = message;
        }

        public override string ToString() => (Channel != null) ? $"{Channel}: {Message}" : Message;
    }
}
