namespace Mallos.Networking.Chat
{
    using Mallos.Networking.Chat.Abstractions;
    using Mallos.Networking.Chat.Packets;
    using Mallos.Networking.User.Abstractions;
    using Networker.Common;
    using Networker.Common.Abstractions;
    using Networker.Server.Abstractions;
    using System;
    using System.Threading.Tasks;

    class ChatServerHandler<TUser> : PacketHandlerBase<ChatPacket>
        where TUser : IdentityUser
    {
        private readonly NetServer<TUser> netPeer;
        private readonly ChatService<TUser> chatService;
        private readonly ITcpConnections tcpConnections;

        public ChatServerHandler(
            NetPeer<TUser> netPeer,
            IChatService<TUser> chatService,
            ITcpConnections tcpConnections)
        {
            this.netPeer = (NetServer<TUser>)netPeer;
            this.chatService = (ChatService<TUser>)chatService;
            this.tcpConnections = tcpConnections;
        }

        public override Task Process(ChatPacket packet, IPacketContext context)
        {
            var sender = tcpConnections.FindByEndpoint(context.Sender.EndPoint);
            var user = (TUser)sender.UserTag;

            this.netPeer.SendPacket(new ChatReplyPacket(Guid.NewGuid(), packet.Message));

            var message = new ChatMessage(user, packet.Channel, packet.Message);
            chatService.InvokeReceived(message);

            return Task.CompletedTask;
        }
    }
}
