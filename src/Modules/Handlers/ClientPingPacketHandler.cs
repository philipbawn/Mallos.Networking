namespace Mallos.Networking.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Mallos.Networking.Packets;
    using Networker.Common;
    using Networker.Common.Abstractions;

    public class ClientPingPacketHandler : PacketHandlerBase<PingPacket>
    {
        public ClientPingPacketHandler(IPacketSerialiser packetSerialiser)
            : base(packetSerialiser)
        {

        }

        public override async Task Process(PingPacket packet, IPacketContext context)
        {
            var diff = DateTime.UtcNow.Subtract(packet.Time);
            Console.WriteLine($"Ping is {diff.Milliseconds}ms");
        }
    }
}
