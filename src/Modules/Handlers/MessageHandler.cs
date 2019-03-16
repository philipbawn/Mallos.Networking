namespace Mallos.Networking.Handlers
{
    using System.Threading.Tasks;
    using Mallos.Networking.Packets;
    using Networker.Common;
    using Networker.Common.Abstractions;

    class MessageHandler : PacketHandlerBase<MessagePacket>
    {
        public MessageHandler(IPacketSerialiser packetSerialiser) 
            : base(packetSerialiser)
        {

        }

        public override Task Process(MessagePacket packet, IPacketContext context)
        {
            System.Console.WriteLine(packet);

            return Task.CompletedTask;
        }
    }
}
