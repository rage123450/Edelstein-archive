using Edelstein.Network.Packets;
using MoreLinq.Extensions;

namespace Edelstein.WvsGame.Conversations.Messages.Requests
{
    public class AskAvatar : ConversationQuestion<byte>
    {
        public override ScriptMessageType MessageType => ScriptMessageType.AskAvatar;

        private readonly string _text;
        private readonly int[] _styles;

        public AskAvatar(byte speakerTypeID, int speakerTemplateID, SpeakerParamType speakerParam, string text,
            int[] styles) : base(speakerTypeID, speakerTemplateID, speakerParam)
        {
            _text = text;
            _styles = styles;
        }

        public override void Encode(OutPacket packet)
        {
            base.Encode(packet);

            packet.Encode<string>(_text);
            packet.Encode<byte>((byte) _styles.Length);
            _styles.ForEach(s => packet.Encode<int>(s));
        }
    }
}