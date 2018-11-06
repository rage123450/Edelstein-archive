using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Conversations.Messages.Requests
{
    public class Say : ConversationQuestion<byte>
    {
        public override ScriptMessageType MessageType => ScriptMessageType.Say;

        private readonly string _text;
        private readonly bool _prev;
        private readonly bool _next;

        public Say(
            byte speakerTypeID,
            int speakerTemplateID,
            SpeakerParamType speakerParam,
            string text,
            bool prev,
            bool next
        ) : base(speakerTypeID, speakerTemplateID, speakerParam)
        {
            _text = text;
            _prev = prev;
            _next = next;
        }

        public override void Encode(OutPacket packet)
        {
            base.Encode(packet);

            if ((SpeakerParam & SpeakerParamType.NPCReplacedByNPC) != 0)
                packet.Encode<int>(SpeakerTemplateID);
            packet.Encode<string>(_text);
            packet.Encode<bool>(_prev);
            packet.Encode<bool>(_next);
        }
    }
}