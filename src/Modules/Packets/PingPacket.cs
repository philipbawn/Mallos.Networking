namespace Mallos.Networking.Packets
{
    using Networker.Formatter.ZeroFormatter;
    using ZeroFormatter;

    [ZeroFormattable]
    public class PingPacket : ZeroFormatterPacketBase
    {
        [Index(2)]
        public virtual System.DateTime Time { get; set; }
    }
}
