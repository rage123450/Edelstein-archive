using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Conversations.Messages.Requests
{
    public class AskNumber : ConversationQuestion<int>
    {
        protected override ScriptMessageType MessageType => ScriptMessageType.AskNumber;

        private readonly string _text;
        private readonly int _def;
        private readonly int _min;
        private readonly int _max;

        public AskNumber(
            byte speakerTypeID,
            int speakerTemplateID,
            SpeakerParamType speakerParam,
            string text,
            int def,
            int min,
            int max
        ) : base(speakerTypeID, speakerTemplateID, speakerParam)
        {
            _text = text;
            _def = def;
            _min = min;
            _max = max;
        }

        public override void Encode(OutPacket packet)
        {
            base.Encode(packet);

            packet.Encode<string>(_text);
            packet.Encode<int>(_def);
            packet.Encode<int>(_min);
            packet.Encode<int>(_max);
        }
    }
}