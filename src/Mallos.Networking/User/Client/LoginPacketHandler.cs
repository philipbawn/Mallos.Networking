namespace Mallos.Networking.User
{
    using Networker.Common;
    using Networker.Common.Abstractions;
    using System.Linq;
    using System.Threading.Tasks;

    class LoginPacketHandler : PacketHandlerBase<LoginPacket>
    {
        public readonly UserManager UserManager;

        public LoginPacketHandler(UserManager userManager)
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
