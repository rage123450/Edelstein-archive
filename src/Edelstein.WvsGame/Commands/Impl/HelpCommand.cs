using System.Threading.Tasks;
using MoreLinq;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class HelpCommand : Command
    {
        public override string Name => "Help";
        public override string Description => "Get the help that you need.";
        private readonly Command parent;

        public HelpCommand(Command parent)
        {
            this.parent = parent;
        }

        protected override Task Execute(CommandContext ctx)
        {
            parent.Commands.ForEach(c => ctx.User.Message(c.Name + " - " + c.Description));
            return Task.CompletedTask;
        }
    }
}