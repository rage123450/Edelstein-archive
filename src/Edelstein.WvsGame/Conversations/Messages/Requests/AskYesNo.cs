using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Conversations.Messages.Requests
{
    public class AskYesNo : ConversationQuestion<byte>
    {
        protected override ScriptMessageType MessageType =>
            _quest
                ? ScriptMessageType.AskAccept
                : ScriptMessageType.AskYesNo;

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