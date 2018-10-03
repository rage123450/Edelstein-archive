using Edelstein.WvsGame.Conversations.Messages;
using Edelstein.WvsGame.Fields.Objects;

namespace Edelstein.WvsGame.Conversations.Speakers
{
    public class FieldNPCSpeaker : Speaker
    {
        private readonly FieldNPC _fieldNPC;

        public override byte SpeakerTypeID => 0;
        public override int SpeakerTemplateID => _fieldNPC.Template.TemplateID;
        public override SpeakerParamType SpeakerParam => 0;

        public FieldNPCSpeaker(
            ConversationContext context,
            FieldNPC fieldNPC
        ) : base(context)
        {
            _fieldNPC = fieldNPC;
        }
    }
}