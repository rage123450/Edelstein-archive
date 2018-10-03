using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Conversations.Messages
{
    public abstract class ConversationQuestion : IEncodable
    {
        protected abstract byte MessageType { get; }

        protected readonly byte SpeakerTypeID;
        protected readonly int SpeakerTemplateID;
        protected readonly SpeakerParamType Param;

        protected ConversationQuestion(byte speakerTypeID, int speakerTemplateID, SpeakerParamType param)
        {
            SpeakerTypeID = speakerTypeID;
            SpeakerTemplateID = speakerTemplateID;
            Param = param;
        }

        public virtual void Encode(OutPacket packet)
        {
            packet.Encode<byte>(SpeakerTypeID);
            packet.Encode<int>(SpeakerTemplateID);
            packet.Encode<byte>(MessageType);
            packet.Encode<byte>((byte) Param);
        }
    }
}