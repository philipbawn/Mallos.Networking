namespace Mallos.Networking.Chat.Command.Commands
{
    using System.Threading.Tasks;
    using Mallos.Networking.User.Abstractions;

    class CommandMsg : Command
    {
        public CommandMsg()
            : base("msg", "Message another user.", "/msg <user> <message>")
        {

        }

        public override Task Execute(IdentityUser sender)
        {
            throw new System.NotImplementedException();
        }
    }
}
