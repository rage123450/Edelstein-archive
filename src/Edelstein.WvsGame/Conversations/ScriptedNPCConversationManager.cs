using System.Linq;
using Edelstein.WvsGame.Conversations.Speakers;
using Edelstein.WvsGame.Fields.Objects;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Conversations
{
    public class ScriptedNPCConversationManager : ScriptedConversationManager<FieldNPC, FieldNPCSpeaker>
    {
        public ScriptedNPCConversationManager(WvsGameOptions options) : base(options)
        {
        }

        public bool Start(FieldUser user, FieldNPC npc)
        {
            return Start(user, npc, npc.Template.Scripts.FirstOrDefault()?.Script);
        }

        public override FieldNPCSpeaker GetSelfSpeaker(ConversationContext context, FieldNPC obj)
            => new FieldNPCSpeaker(context, obj);
    }
}