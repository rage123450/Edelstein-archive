using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Conversations.Messages.Requests
{
    public class AskText : ConversationQuestion<string>
    {
        public override ScriptMessageType MessageType => ScriptMessageType.AskText;

        private readonly string _text;
        private readonly string _textDefault;
        private readonly short _lenMin;
        private readonly short _lenMax;

        public AskText(
            byte speakerTypeID,
            int speakerTemplateID,
            SpeakerParamType speakerParam,
            string text,
            string textDefault,
            short lenMin,
            short lenMax
        ) : base(speakerTypeID, speakerTemplateID, speakerParam)
        {
            _text = text;
            _textDefault = textDefault;
            _lenMin = lenMin;
            _lenMax = lenMax;
        }

        public override void Encode(OutPacket packet)
        {
            base.Encode(packet);

            packet.Encode<string>(_text);
            packet.Encode<string>(_textDefault);
            packet.Encode<short>(_lenMin);
            packet.Encode<short>(_lenMax);
        }
    }
}