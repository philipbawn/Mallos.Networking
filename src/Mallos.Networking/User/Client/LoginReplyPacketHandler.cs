namespace Mallos.Networking.User
{
    using Networker.Common;
    using Networker.Common.Abstractions;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

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
