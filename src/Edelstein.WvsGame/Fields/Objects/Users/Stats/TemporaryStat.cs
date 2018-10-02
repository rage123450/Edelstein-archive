using System;
using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Fields.Objects.Users.Stats
{
    public class TemporaryStat : IEncodable
    {
        public TemporaryStatType Type { get; set; }
        public short Option { get; set; }
        public int TemplateID { get; set; }
        public DateTime DateExpire { get; set; }

        public void Encode(OutPacket packet)
        {
            packet.Encode<short>(Option);
            packet.Encode<int>(TemplateID);
            packet.Encode<int>((int) (DateExpire - DateTime.Now).TotalMilliseconds);
        }
    }
}