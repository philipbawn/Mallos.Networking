namespace Mallos.Networking.User.Packets
{
    using Networker.Formatter.ZeroFormatter;
    using System.ComponentModel;
    using ZeroFormatter;

    [ZeroFormattable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class LoginReplyPacket : ZeroFormatterPacketBase
    {
        [Index(2)]
        public virtual bool Accepted { get; set; }

        [Index(3)]
        public virtual string[] Errors { get; set; }

        public LoginReplyPacket() { }
        public LoginReplyPacket(bool accepted, string[] errors = null)
        {
            this.Accepted = accepted;
            this.Errors = errors;
        }
    }
}
