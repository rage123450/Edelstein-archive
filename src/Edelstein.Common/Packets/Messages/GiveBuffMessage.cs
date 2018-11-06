using Edelstein.Network.Packets;

namespace Edelstein.Common.Packets.Messages
{
    public class GiveBuffMessage : Message
    {
        protected override byte Type => 0x8;
        private readonly int _templateID;

        public GiveBuffMessage(int templateId)
        {
            _templateID = templateId;
        }

        public override void Encode(OutPacket packet)
        {
            base.Encode(packet);

            packet.Encode<int>(_templateID);
        }
    }
}