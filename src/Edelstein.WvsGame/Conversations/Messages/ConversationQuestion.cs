using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Conversations.Messages
{
    public abstract class ConversationQuestion<T> : IEncodable
    {
        protected abstract ScriptMessageType MessageType { get; }

        protected readonly byte SpeakerTypeID;
        protected readonly int SpeakerTemplateID;
        protected readonly SpeakerParamType SpeakerParam;

        protected ConversationQuestion(byte speakerTypeID, int speakerTemplateID, SpeakerParamType speakerParam)
        {
            SpeakerTypeID = speakerTypeID;
            SpeakerTemplateID = speakerTemplateID;
            SpeakerParam = speakerParam;
        }

        public virtual void Encode(OutPacket packet)
        {
            packet.Encode<byte>(SpeakerTypeID);
            packet.Encode<int>(SpeakerTemplateID);
            packet.Encode<byte>((byte) MessageType);
            packet.Encode<byte>((byte) SpeakerParam);
        }
    }
}