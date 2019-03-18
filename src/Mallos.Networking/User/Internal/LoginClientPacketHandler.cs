namespace Mallos.Networking.User
{
    using Networker.Common;
    using Networker.Common.Abstractions;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    class LoginPacketHandler<TUser> : PacketHandlerBase<LoginPacket>
        where TUser : IdentityUser
    {
        public readonly NetPeer NetPeer;
        public readonly UserManager<TUser> UserManager;

        public LoginPacketHandler(NetPeer netPeer, UserManager<TUser> userManager)
        {
            this.NetPeer = netPeer;
            this.UserManager = userManager;
        }

        public override async Task Process(LoginPacket packet, IPacketContext context)
        {
            var identityResult = await UserManager.AddLoginAsync(packet.Username, packet.Password);

            var errors = (identityResult.Errors != null) ? identityResult.Errors.ToArray() : new string[0];
            var replyPacket = new LoginReplyPacket(identityResult.Success, errors);

            context.Sender.Send(replyPacket);

            if (replyPacket.Accepted)
            {
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
