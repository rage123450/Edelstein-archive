using System.Threading;
using Edelstein.WvsGame.Conversations.Messages;
using Edelstein.WvsGame.Conversations.Speakers;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Conversations
{
    public class DefaultConversationManager : ConversationManager<FieldUserSpeaker, NPCSpeaker>
    {
        public bool Start(FieldUser user, Conversation<FieldUserSpeaker, NPCSpeaker> conversation,
            int templateID = 9010000, SpeakerParamType type = SpeakerParamType.NPCReplacedByNPC)
        {
            var context = new ConversationContext(new CancellationTokenSource(), user.Socket);

            return Start(
                new FieldUserSpeaker(context, user),
                new NPCSpeaker(context, templateID, type),
                conversation
            );
        }
    }
}