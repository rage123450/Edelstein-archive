using System;
using System.IO;
using System.Linq;
using System.Threading;
using Edelstein.WvsGame.Conversations.Speakers;
using Edelstein.WvsGame.Fields.Objects;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Conversations
{
    public class NPCConversationManager : ConversationManager<FieldUser, FieldNPC>
    {
        private readonly WvsGameOptions _options;

        public NPCConversationManager(WvsGameOptions options)
        {
            _options = options;
        }

        public override bool Start(FieldUser target, FieldNPC self)
        {
            var script = self.Template.Scripts.FirstOrDefault()?.Script;

            if (target.ConversationContext != null) return false;
            if (script == null) return false;

            script = Path.Combine(_options.ScriptPath, $"{script}.lua");

            if (!File.Exists(script)) return false;

            var context = new ConversationContext(new CancellationTokenSource(), target.Socket);
            var conversation = new ScriptedConversation<FieldUserSpeaker, FieldNPCSpeaker>(script);

            target.ConversationContext = context;
            conversation.Start(
                context,
                new FieldUserSpeaker(context, target),
                new FieldNPCSpeaker(context, self)
            ).ContinueWith(t =>
            {
                target.ConversationContext = null;
                target.ModifyStats(exclRequest: true);
            });
            return true;
        }
    }
}