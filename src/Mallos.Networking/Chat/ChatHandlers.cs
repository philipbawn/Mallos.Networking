namespace Mallos.Networking.Chat
{
    using Mallos.Networking.User;
    using Networker.Common;
    using Networker.Common.Abstractions;
    using Networker.Formatter.ZeroFormatter;
    using Networker.Server.Abstractions;
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using ZeroFormatter;

    /// <summary>
    /// This packet is sent from the client.
    /// </summary>
    [ZeroFormattable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ChatPacket : ZeroFormatterPacketBase
    {
        [Index(2)]
        public virtual string Channel { get; set; }

        [Index(3)]
        public virtual string Message { get; set; }

        public ChatPacket() { }
        public ChatPacket(string channel, string message)
        {
            this.Channel = channel;
            this.Message = message;
        }

        public override string ToString() => $"{Channel}: {Message}";
    }

    /// <summary>
    /// This packet is sent from the server.
    /// </summary>
    [ZeroFormattable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ChatReplyPacket : ZeroFormatterPacketBase
    {
        [Index(2)]
        public virtual Guid Sender { get; set; }

        [Index(3)]
        public virtual string Message { get; set; }

        public ChatReplyPacket() { }
        public ChatReplyPacket(Guid sender, string message)
        {
            this.Sender = sender;
            this.Message = message;
        }

        public override string ToString() => $"{Sender}: {Message}";
    }

    /// <summary>
    /// Client gets the <see cref="ChatReplyPacket"/> which contains sender id.
    /// </summary>
    class ChatReplyPacketHandler : PacketHandlerBase<ChatReplyPacket>
    {
        public readonly NetPeer NetPeer;
        public readonly ChatService ChatService;

        public ChatReplyPacketHandler(NetPeer netPeer, IChatService chatService)
        {
            this.NetPeer = netPeer;

            // NOTE: Not the best code, lets see if we can make it better later.
            this.ChatService = (ChatService)chatService;
        }

        public override Task Process(ChatReplyPacket packet, IPacketContext context)
        {
            // TODO: Get sender User

            var message = new ChatMessage(null, "", packet.Message);
            ChatService.InvokeReceived(message);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Server gets the <see cref="ChatPacket"/> from the client that has raw data.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    class ChatPacketHandler<TUser> : PacketHandlerBase<ChatPacket>
        where TUser : IdentityUser
    {
        private readonly NetServer<TUser> netPeer;
        private readonly ChatService chatService;
        private readonly ITcpConnections tcpConnections;

        public ChatPacketHandler(
            NetPeer netPeer, 
            IChatService chatService,
            ITcpConnections tcpConnections)
        {
            this.netPeer = (NetServer<TUser>)netPeer;
            this.tcpConnections = tcpConnections;

            // NOTE: Not the best code, lets see if we can make it better later.
            this.chatService = (ChatService)chatService;
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
