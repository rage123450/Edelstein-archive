using System.Collections.Generic;
using Edelstein.Network.Packets;
using MoreLinq.Extensions;

namespace Edelstein.Common.Packets.Messages
{
    public class GeneralItemExpireMessage : Message
    {
        protected override byte Type => 0x9;
        private readonly ICollection<int> _templates;

        public GeneralItemExpireMessage(ICollection<int> templates)
        {
            _templates = templates;
        }

        public override void Encode(OutPacket packet)
        {
            base.Encode(packet);
            
            packet.Encode<byte>((byte) _templates.Count);
            _templates.ForEach(s => packet.Encode<int>(s));
        }
    }
}