using System;
using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Fields.Objects.Users.Stats
{
    public class TemporaryStatEntry
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

        public void EncodeRemote(OutPacket packet, int size)
        {
            switch (size)
            {
                case 1:
                    packet.Encode<byte>((byte) Option);
                    break;
                case 2:
                    packet.Encode<short>(Option);
                    break;
                case 4:
                    packet.Encode<int>(Option);
                    break;
            }
        }
    }
}