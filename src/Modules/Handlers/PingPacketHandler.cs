namespace Mallos.Networking.Handlers
{
    using Mallos.Networking.Packets;
    using Microsoft.Extensions.Logging;
    using Networker.Common;
    using Networker.Common.Abstractions;
    using System.Threading.Tasks;

    public class PingPacketHandler : PacketHandlerBase<PingPacket>
    {
        public override async Task Process(PingPacket packet, IPacketContext context)
        {
            System.Console.WriteLine($"[{packet.Time}]: Received a ping packet from {context.Sender.EndPoint}");
        }
    }
}
