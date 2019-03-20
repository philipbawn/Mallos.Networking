namespace Mallos.Networking.Chat.Command
{
    using Mallos.Networking.User.Abstractions;
    using System;
    using System.Threading.Tasks;

    public abstract class Command
    {
        public readonly string Name;
        public readonly string Description;
        public readonly string Usage;

        protected Command(string name, string description, string usage)
        {
            this.Name        = name ?? throw new ArgumentNullException(nameof(name));
            this.Description = description ?? throw new ArgumentNullException(nameof(description));
            this.Usage       = usage ?? throw new ArgumentNullException(nameof(usage));
        }

        public abstract Task Execute(IdentityUser sender);
    }
}
