using System.Linq;
using System.Threading.Tasks;
using MoreLinq.Extensions;

namespace Edelstein.WvsGame.Commands.Impl.Util
{
    public class AliasCommand : Command
    {
        public override string Name => "Aliases";
        public override string Description => "Get the aliases that you need.";
        private readonly Command parent;

        public AliasCommand(Command parent)
        {
            this.parent = parent;
        }

        protected override Task Execute(CommandContext ctx)
        {
            parent.Commands
                .Where(c => c.Aliases.Count > 0)
                .ForEach(c => ctx.User.Message(c.Name + " - " + string.Join(", ", c.Aliases)));
            return Task.CompletedTask;
        }
    }
}