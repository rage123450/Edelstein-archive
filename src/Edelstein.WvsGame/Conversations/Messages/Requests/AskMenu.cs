using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Conversations.Messages.Requests
{
    public class AskMenu : ConversationQuestion<int>
    {
        public override ScriptMessageType MessageType => ScriptMessageType.AskMenu;

        private readonly string _text;

        public AskMenu(
            byte speakerTypeID,
            int speakerTemplateID,
            SpeakerParamType speakerParam,
            string text
        ) : base(speakerTypeID, speakerTemplateID, speakerParam)
        {
            _text = text;
        }

        public override void Encode(OutPacket packet)
        {
            base.Encode(packet);

            packet.Encode<string>(_text);
        }
    }
}