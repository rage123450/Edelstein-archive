using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.WvsGame.Conversations.Speakers;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Conversations
{
    public abstract class ScriptedConversationManager<T, S> : ConversationManager<FieldUserSpeaker, S>
        where S : Speaker
    {
        private readonly WvsGameOptions _options;

        public ScriptedConversationManager(WvsGameOptions options)
        {
            _options = options;
        }

        public Task<bool> Start(FieldUser user, T self, string script)
        {
            if (script == null) return Task.FromResult(false);

            script = Path.Combine(_options.ScriptPath, $"{script}.lua");

            if (!File.Exists(script))
            {
                user
                    .Message("The script " + Path.GetFileNameWithoutExtension(script) + " does not exist.")
                    .ContinueWith(t => user.ModifyStats(exclRequest: true));
                return Task.FromResult(false);
            }

            var context = new ConversationContext(user.Socket);

            return Start(
                new FieldUserSpeaker(context, user),
                GetSelfSpeaker(context, self),
                new ScriptedConversation<FieldUserSpeaker, S>(context, script)
            );
        }

        public abstract S GetSelfSpeaker(ConversationContext context, T obj);
    }
}