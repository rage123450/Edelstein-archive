using System.Threading.Tasks;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Commands
{
    public class CommandRegistry : Command<ICommandOption>
    {
        public override string Name => "Registry";
        public override string Description => "The registry that holds all commands.";

        public override Task Execute(FieldUser user, ICommandOption option)
        {
            return Task.CompletedTask;
        }
    }
}