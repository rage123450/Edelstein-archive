using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Conversations.Messages.Requests
{
    public class AskSlideMenu : ConversationQuestion<int>
    {
        public override ScriptMessageType MessageType => ScriptMessageType.AskSlideMenu;

        private readonly int _type;
        private readonly int _selected;
        private readonly string _text;

        public AskSlideMenu(
            byte speakerTypeID,
            int speakerTemplateID,
            SpeakerParamType speakerParam,
            int type,
            int selected,
            string text) : base(speakerTypeID, speakerTemplateID, speakerParam)
        {
            _type = type;
            _selected = selected;
            _text = text;
        }

        public override void Encode(OutPacket packet)
        {
            base.Encode(packet);

            packet.Encode<int>(_type);
            packet.Encode<int>(_selected);
            packet.Encode<string>(_text);
        }
    }
}