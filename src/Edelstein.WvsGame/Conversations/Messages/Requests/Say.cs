using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Conversations.Messages.Requests
{
    public class Say : ConversationQuestion
    {
        protected override byte MessageType => 0x0;

        private readonly string _message;
        private readonly bool _prev;
        private readonly bool _next;

        public Say(
            byte speakerTypeID,
            int speakerTemplateID,
            SpeakerParamType param,
            string message,
            bool prev = false,
            bool next = false
        ) : base(speakerTypeID, speakerTemplateID, param)
        {
            _message = message;
            _prev = prev;
            _next = next;
        }

        public override void Encode(OutPacket packet)
        {
            base.Encode(packet);

            if ((Param & SpeakerParamType.NPCReplacedByNPC) != 0)
                packet.Encode<int>(SpeakerTemplateID);
            packet.Encode<string>(_message);
            packet.Encode<bool>(_prev);
            packet.Encode<bool>(_next);
        }
    }
}