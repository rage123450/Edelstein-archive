using Edelstein.WvsGame.Conversations.Messages;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Conversations.Speakers
{
    public class FieldUserSpeaker : Speaker
    {
        public readonly FieldUser FieldUser;
        
        public override byte SpeakerTypeID => 0;
        public override int SpeakerTemplateID => 9010000;
        public override SpeakerParamType SpeakerParam => SpeakerParamType.NPCReplacedByUser;

        public FieldUserSpeaker(
            ConversationContext context,
            FieldUser fieldUser
        ) : base(context)
        {
            FieldUser = fieldUser;
        }
    }
}