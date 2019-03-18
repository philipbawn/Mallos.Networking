namespace Mallos.Networking.User
{
    using Microsoft.Extensions.Logging;
    using Networker.Common;
    using Networker.Common.Abstractions;
    using Networker.Formatter.ZeroFormatter;
    using Networker.Server.Abstractions;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
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

    class LoginPacketHandler<TUser> : PacketHandlerBase<LoginPacket>
        where TUser : IdentityUser
    {
        private readonly NetServer<TUser> NetPeer;
        private readonly UserManager<TUser> UserManager;
        private readonly ITcpConnections TcpConnections;

        public LoginPacketHandler(
            NetPeer netPeer,
            UserManager<TUser> userManager,
            ITcpConnections tcpConnections)
        {
            this.NetPeer = (NetServer<TUser>)netPeer;
            this.UserManager = userManager;
            this.TcpConnections = tcpConnections;
        }

        public override async Task Process(LoginPacket packet, IPacketContext context)
        {
            var identityResult = await UserManager.AddLoginAsync(packet.Username, packet.Password);

            var errors = (identityResult.Errors != null) ? identityResult.Errors.ToArray() : new string[0];
            var replyPacket = new LoginReplyPacket(identityResult.Success, errors);

            context.Sender.Send(replyPacket);

            if (replyPacket.Accepted)
            {
                var user = await UserManager.UserStorage.FindByNameAsync(packet.Username);

                var userConnection = TcpConnections.FindByEndpoint(context.Sender.EndPoint);
                userConnection.UserTag = user;

                NetPeer.OnClientConnected(user);

                this.NetPeer.Logger.LogInformation("User '{username}' authenticated ({address}).",
                    packet.Username, context.Sender.EndPoint.ToString());
            }
            else
            {
                this.NetPeer.Logger.LogInformation("User '{username}' failed to authenticated ({address}).",
                    packet.Username, context.Sender.EndPoint.ToString());
            }
        }
    }

    class LoginReplyPacketHandler : PacketHandlerBase<LoginReplyPacket>
    {
        public readonly NetClient NetPeer;

        public LoginReplyPacketHandler(NetPeer netPeer)
        {
            this.NetPeer = (NetClient)netPeer;
        }

        public override Task Process(LoginReplyPacket packet, IPacketContext context)
        {
            if (packet.Accepted)
            {
                this.NetPeer.Logger.LogInformation("User is authenticated.");
                this.NetPeer.status = NetPeerStatus.Online;
            }
            else
            {
                this.NetPeer.Logger.LogWarning("Authentication Errors: {errors}",
                    string.Join(", ", packet.Errors));
                this.NetPeer.Stop();
            }
            return Task.CompletedTask;
        }
    }
}
