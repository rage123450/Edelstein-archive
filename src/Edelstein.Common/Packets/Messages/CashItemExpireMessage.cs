using Edelstein.Network.Packets;

namespace Edelstein.Common.Packets.Messages
{
    public class CashItemExpireMessage : Message
    {
        protected override byte Type => 0x2;
        private readonly int _templateID;

        public CashItemExpireMessage(int templateId)
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