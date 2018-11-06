using Edelstein.WvsGame.Conversations.Messages;

namespace Edelstein.WvsGame.Conversations.Speakers
{
    public class NPCSpeaker : Speaker
    {
        public override byte SpeakerTypeID => 0;
        public override int SpeakerTemplateID { get; }
        public override SpeakerParamType SpeakerParam => SpeakerParamType.NPCReplacedByNPC;

        public NPCSpeaker(ConversationContext context, int speakerTemplateID) : base(context)
        {
            SpeakerTemplateID = speakerTemplateID;
        }
    }
}