namespace Mallos.Networking.User
{
    using Networker.Common;
    using Networker.Common.Abstractions;
    using System.Threading.Tasks;

    class LoginReplyPacketHandler : PacketHandlerBase<LoginPacket>
    {
        public override Task Process(LoginPacket packet, IPacketContext context)
        {
            return Task.CompletedTask;
        }
    }
}
