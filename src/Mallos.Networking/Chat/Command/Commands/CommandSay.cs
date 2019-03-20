namespace Mallos.Networking.Chat.Command.Commands
{
    using System.Threading.Tasks;
    using Mallos.Networking.User.Abstractions;

    class CommandSay : Command
    {
        public CommandSay()
            : base("say", "Send a message to everyone", "/say <message>")
        {

        }

        public override Task Execute(IdentityUser sender)
        {
            throw new System.NotImplementedException();
        }
    }
}
