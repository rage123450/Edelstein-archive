using System.Threading.Tasks;
using Edelstein.WvsGame.Conversations.Messages;
using Edelstein.WvsGame.Conversations.Speakers;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Conversations
{
    public class ScriptedDefaultConversationManager : ScriptedConversationManager<int, NPCSpeaker>
    {
        public ScriptedDefaultConversationManager(WvsGameOptions options) : base(options)
        {
        }

        public Task Start(FieldUser user, string script, int templateID = 9010000)
        {
            return Start(user, templateID, script);
        }

        public override NPCSpeaker GetSelfSpeaker(ConversationContext context, int obj)
            => new NPCSpeaker(context, obj, SpeakerParamType.NPCReplacedByUser);
    }
}