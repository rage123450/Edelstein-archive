using Edelstein.Network.Packets;

namespace Edelstein.Common.Packets.Messages
{
    public class SystemMessage : Message
    {
        protected override byte Type => 0xA;
        private readonly string _message;

        public SystemMessage(string message)
        {
            _message = message;
        }

        public override void Encode(OutPacket packet)
        {
            base.Encode(packet);

            packet.Encode<string>(_message);
        }
    }
}