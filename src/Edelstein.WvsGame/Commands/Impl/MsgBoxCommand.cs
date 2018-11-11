using System;
using System.Threading.Tasks;
using Edelstein.WvsGame.Fields.Objects;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class MsgBoxCommand : Command
    {
        public override string Name => "msgbox";
        public override string Description => "Flies a message box in the current field position";

        protected override Task Execute(CommandContext ctx)
        {
            var templateID = Convert.ToInt32(ctx.Args.Dequeue());
            var hope = ctx.Args.Dequeue();
            var messageBox = new FieldMessageBox(
                templateID,
                hope,
                ctx.User.Character.Name,
                DateTime.Now.AddSeconds(10)
            )
            {
                X = ctx.User.X,
                Y = ctx.User.Y
            };

            ctx.User.Field.Enter(messageBox);
            return Task.CompletedTask;
        }
    }
}