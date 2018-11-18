using Edelstein.WvsGame.Conversations.Messages;
using Edelstein.WvsGame.Fields.Objects;

namespace Edelstein.WvsGame.Conversations.Speakers
{
    public class FieldNPCSpeaker : Speaker
    {
        public readonly FieldNPC FieldNPC;

        public override byte SpeakerTypeID => 0;
        public override int SpeakerTemplateID => FieldNPC.Template.TemplateID;
        public override SpeakerParamType SpeakerParam => 0;

        public FieldNPCSpeaker(
            ConversationContext context,
            FieldNPC fieldNPC
        ) : base(context)
        {
            FieldNPC = fieldNPC;
        }
    }
}