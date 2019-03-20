namespace Mallos.Networking.Chat
{
    using Mallos.Networking.Chat.Abstractions;
    using Mallos.Networking.Chat.Packets;
    using Mallos.Networking.User.Abstractions;
    using Networker.Common;
    using Networker.Common.Abstractions;
    using System.Threading.Tasks;

    class ChatReceiverHandler<TUser> : PacketHandlerBase<ChatReplyPacket>
        where TUser : IdentityUser
    {
        public readonly NetPeer<TUser> NetPeer;
        public readonly ChatService<TUser> ChatService;

        public ChatReceiverHandler(
            NetPeer<TUser> netPeer,
            IChatService<TUser> chatService)
        {
            this.NetPeer = netPeer;
            this.ChatService = (ChatService<TUser>)chatService;
        }

        public override Task Process(ChatReplyPacket packet, IPacketContext context)
        {
            // TODO: Get sender User

            var message = new ChatMessage(null, "", packet.Message);
            ChatService.InvokeReceived(message);
            return Task.CompletedTask;
        }
    }
}
