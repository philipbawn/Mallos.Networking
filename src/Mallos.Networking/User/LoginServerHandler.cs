namespace Mallos.Networking.User
{
    using Mallos.Networking.User.Abstractions;
    using Mallos.Networking.User.Packets;
    using Microsoft.Extensions.Logging;
    using Networker.Common;
    using Networker.Common.Abstractions;
    using Networker.Server.Abstractions;
    using System.Linq;
    using System.Threading.Tasks;

    class LoginServerHandler<TUser> : PacketHandlerBase<LoginPacket>
        where TUser : IdentityUser
    {
        private readonly NetServer<TUser> NetPeer;
        private readonly UserManager<TUser> UserManager;
        private readonly ITcpConnections TcpConnections;

        public LoginServerHandler(
            NetPeer<TUser> netPeer,
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
}
