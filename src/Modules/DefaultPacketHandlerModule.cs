namespace Mallos.Networking.Handlers
{
    using System;
    using Mallos.Networking.Packets;
    using Networker.Common;

    public class DefaultPacketHandlerModule : PacketHandlerModuleBase
    {
        public DefaultPacketHandlerModule()
        {
            this.AddPacketHandler<PingPacket, PingPacketHandler>();
        }
    }
}
