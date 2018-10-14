using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Fields.Attacking
{
    public class AttackInfoEntry : IDecodable
    {
        public int MobID { get; set; }
        public byte HitAction { get; set; }
        public bool IsLeft { get; set; }
        public byte FrameIDx { get; set; }
        public int[] Damage { get; set; }

        public void Decode(InPacket packet) => Decode(packet, 0);

        public void Decode(InPacket packet, int damagePerMob)
        {
            MobID = packet.Decode<int>();
            HitAction = packet.Decode<byte>();
            IsLeft =  packet.Decode<bool>(); // TODO: mask
            FrameIDx = packet.Decode<byte>();
            packet.Decode<byte>(); // CalcDamageStatIndex & Doomed
            packet.Decode<short>(); // X, Y
            packet.Decode<short>();
            packet.Decode<short>(); // X, Y
            packet.Decode<short>();
            packet.Decode<short>(); // Delay

            Damage = new int[damagePerMob];
            for (var i = 0; i < damagePerMob; i++)
                Damage[i] = packet.Decode<int>();

            packet.Decode<int>(); // Crc
        }
    }
}