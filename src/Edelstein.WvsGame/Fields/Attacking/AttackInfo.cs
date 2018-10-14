using System;
using System.Collections.Generic;
using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Fields.Attacking
{
    public class AttackInfo : IDecodable
    {
        public int DamagePerMob { get; set; }
        public int Count { get; set; }
        public int SkillID { get; set; }
        public byte SkillLevel { get; set; }
        public short AttackAction { get; set; }
        public bool IsLeft { get; set; }
        public byte AttackActionType { get; set; }
        public byte AttackSpeed { get; set; }
        public int AttackTime { get; set; }

        public ICollection<AttackInfoEntry> Entries { get; set; }

        public void Decode(InPacket packet)
        {
            packet.Decode<byte>();
            packet.Decode<int>();
            packet.Decode<int>();

            var damagePerMobAndCount = packet.Decode<byte>();
            DamagePerMob = damagePerMobAndCount & 0xF;
            Count = damagePerMobAndCount >> 4;

            packet.Decode<int>();
            packet.Decode<int>();
            SkillID = packet.Decode<int>();
            SkillLevel = packet.Decode<byte>(); // is this right?
            packet.Decode<int>();
            packet.Decode<int>();
            packet.Decode<int>();
            packet.Decode<int>();
            // if Keydown - int // KeyDown
            packet.Decode<byte>();

            var attackActionAndIsLeft = packet.Decode<short>();
            AttackAction = (short) (attackActionAndIsLeft & 0x7FFF);
            IsLeft = ((attackActionAndIsLeft >> 15) & 1) != 0;

            packet.Decode<int>();
            AttackActionType = packet.Decode<byte>();
            AttackSpeed = packet.Decode<byte>();
            AttackTime = packet.Decode<int>();
            packet.Decode<int>();

            Entries = new List<AttackInfoEntry>();
            for (var i = 0; i < Count; i++)
            {
                var entry = new AttackInfoEntry();

                entry.Decode(packet, DamagePerMob);
                Entries.Add(entry);
            }

            packet.Decode<short>();
            packet.Decode<short>();
            // if Grenade - short, short // Position
        }
    }
}