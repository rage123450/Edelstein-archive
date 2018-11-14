using System.IO;
using System.Threading;
using Edelstein.WvsGame.Conversations.Messages;
using Edelstein.WvsGame.Conversations.Speakers;
using Edelstein.WvsGame.Fields.Objects;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Conversations
{
    public class ReactorConversationManager: ConversationManager<FieldUser, FieldReactor>
    {
        private readonly WvsGameOptions _options;

        public ReactorConversationManager(WvsGameOptions options)
        {
            _options = options;
        }
        
        public override bool Start(FieldUser target, FieldReactor self)
        {
            var script = self.Template.Action;

            if (target.ConversationContext != null) return false;
            if (script == null) return false;

            script = Path.Combine(_options.ScriptPath, $"{script}.lua");

            if (!File.Exists(script)) return false;

            var context = new ConversationContext(new CancellationTokenSource(), target.Socket);
            var conversation = new ScriptedConversation<FieldUserSpeaker, NPCSpeaker>(script);

            target.ConversationContext = context;
            conversation.Start(
                context,
                new FieldUserSpeaker(context, target),
                new NPCSpeaker(context, 9010000, SpeakerParamType.NPCReplacedByNPC)
            ).ContinueWith(t =>
            {
                target.ConversationContext = null;
                target.ModifyStats(exclRequest: true);
            });
            return true;
        }
    }
}