namespace Mallos.Networking.User
{
    using Networker.Common;
    using Networker.Common.Abstractions;
    using System.Linq;
    using System.Threading.Tasks;

    class LoginPacketHandler<TUser> : PacketHandlerBase<LoginPacket>
        where TUser : IdentityUser
    {
        public readonly UserManager<TUser> UserManager;

        public LoginPacketHandler(UserManager<TUser> userManager)
        {
            this.UserManager = userManager;
        }

        public override async Task Process(LoginPacket packet, IPacketContext context)
        {
            var identityResult = await UserManager.AddLoginAsync(packet.Username, packet.Password);
            var replyPacket = new LoginReplyPacket(identityResult.Success, identityResult.Errors.ToArray());

            context.Sender.Send(replyPacket);
        }
    }
}
