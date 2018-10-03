using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Conversations.Messages.Requests
{
    public class AskYesNo : ConversationQuestion
    {
        protected override byte MessageType => (byte) (_quest ? 0xD : 0x2);

        private readonly string _text;
        private readonly bool _quest;

        public AskYesNo(
            byte speakerTypeID,
            int speakerTemplateID,
            SpeakerParamType speakerParam,
            string text,
            bool quest
        ) : base(speakerTypeID, speakerTemplateID, speakerParam)
        {
            _text = text;
            _quest = quest;
        }

        public override void Encode(OutPacket packet)
        {
            base.Encode(packet);

            packet.Encode<string>(_text);
        }
    }
}