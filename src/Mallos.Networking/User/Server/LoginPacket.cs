namespace Mallos.Networking.User
{
    using Networker.Formatter.ZeroFormatter;
    using System.ComponentModel;
    using ZeroFormatter;

    [ZeroFormattable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class LoginPacket : ZeroFormatterPacketBase
    {
        [Index(2)]
        public virtual string Username { get; set; }

        [Index(3)]
        public virtual string Password { get; set; }

        public LoginPacket() { }
        public LoginPacket(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}
