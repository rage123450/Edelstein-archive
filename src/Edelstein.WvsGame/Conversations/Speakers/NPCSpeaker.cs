using Edelstein.WvsGame.Conversations.Messages;

namespace Edelstein.WvsGame.Conversations.Speakers
{
    public class NPCSpeaker : Speaker
    {
        public override byte SpeakerTypeID => 0;
        public override int SpeakerTemplateID { get; }
        public override SpeakerParamType SpeakerParam { get; }

        public NPCSpeaker(ConversationContext context, int speakerTemplateID, SpeakerParamType speakerParam) :
            base(context)
        {
            SpeakerTemplateID = speakerTemplateID;
            SpeakerParam = speakerParam;
        }
    }
}