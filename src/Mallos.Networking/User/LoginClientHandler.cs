namespace Mallos.Networking.User
{
    using Mallos.Networking.User.Abstractions;
    using Mallos.Networking.User.Packets;
    using Microsoft.Extensions.Logging;
    using Networker.Common;
    using Networker.Common.Abstractions;
    using System.Threading.Tasks;

    class LoginClientHandler : PacketHandlerBase<LoginReplyPacket>
    {
        public readonly NetClient NetPeer;

        public LoginClientHandler(NetPeer<IdentityUser> netPeer)
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
