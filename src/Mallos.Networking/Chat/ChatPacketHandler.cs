namespace Mallos.Networking.Chat
{
    using Networker.Common;
    using Networker.Common.Abstractions;
    using System.Threading.Tasks;

    class ChatPacketHandler : PacketHandlerBase<ChatPacket>
    {
        private readonly ChatService chatService;

        public ChatPacketHandler(IChatService chatService)
        {
            // Not the best code, lets see if we can make it better later.
            this.chatService = (ChatService)chatService;
        }

        public override Task Process(ChatPacket packet, IPacketContext context)
        {
            var message = new ChatMessage(packet.Channel, packet.Message);

            chatService.InvokeReceived(message);

            return Task.CompletedTask;
        }
    }
}
