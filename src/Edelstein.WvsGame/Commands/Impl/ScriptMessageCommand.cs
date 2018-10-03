using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.WvsGame.Conversations;
using Edelstein.WvsGame.Conversations.Speakers;
using Edelstein.WvsGame.Fields.Objects;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class ScriptMessageCommand : Command
    {
        public override string Name => "Script";
        public override string Description => "Tests script message packet";

        protected override Task Execute(CommandContext ctx)
        {
            var context = new ConversationContext(new CancellationTokenSource(), ctx.User.Socket);
            var conversation = new ScriptedConversation<FieldUserSpeaker, FieldNPCSpeaker>("test.lua");

            conversation.Start(
                context,
                new FieldUserSpeaker(context, ctx.User),
                new FieldNPCSpeaker(context, ctx.User.Field.Objects
                    .OfType<FieldNPC>()
                    .First())
            );

            return Task.CompletedTask;
        }
    }
}