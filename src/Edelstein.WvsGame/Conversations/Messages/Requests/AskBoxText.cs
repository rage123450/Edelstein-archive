using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Conversations.Messages.Requests
{
    public class AskBoxText : ConversationQuestion
    {
        protected override byte MessageType => 0xE;

        private readonly string _text;
        private readonly string _textDefault;
        private readonly short _col;
        private readonly short _line;

        public AskBoxText(
            byte speakerTypeID,
            int speakerTemplateID,
            SpeakerParamType speakerParam,
            string text,
            string textDefault,
            short col,
            short line
        ) : base(speakerTypeID, speakerTemplateID, speakerParam)
        {
            _text = text;
            _textDefault = textDefault;
            _col = col;
            _line = line;
        }

        public override void Encode(OutPacket packet)
        {
            base.Encode(packet);

            packet.Encode<string>(_text);
            packet.Encode<string>(_textDefault);
            packet.Encode<short>(_col);
            packet.Encode<short>(_line);
        }
    }
}