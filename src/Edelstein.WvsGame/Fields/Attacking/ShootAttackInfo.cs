using System.Collections.Generic;
using Edelstein.Database.Entities;
using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Fields.Attacking
{
    public class ShootAttackInfo : AttackInfo
    {
        public ShootAttackInfo(Character character) : base(character)
        {
        }

        public override void Decode(InPacket packet)
        {
            packet.Decode<int>();
            packet.Decode<int>();

            var damagePerMobAndCount = packet.Decode<byte>();
            DamagePerMob = damagePerMobAndCount & 0xF;
            Count = damagePerMobAndCount >> 4;

            packet.Decode<int>();
            packet.Decode<int>();
            SkillID = packet.Decode<int>();
            packet.Decode<byte>();
            packet.Decode<int>();
            packet.Decode<int>();
            packet.Decode<long>();
            // if Keydown - int // KeyDown
            packet.Decode<byte>();
            packet.Decode<byte>();

            var attackActionAndIsLeft = packet.Decode<short>();
            AttackAction = (short) (attackActionAndIsLeft & 0x7FFF);
            IsLeft = ((attackActionAndIsLeft >> 15) & 1) != 0;

            packet.Decode<int>();
            AttackActionType = packet.Decode<byte>();
            AttackSpeed = packet.Decode<byte>();
            AttackTime = packet.Decode<int>();
            packet.Decode<int>();

            packet.Decode<short>();
            packet.Decode<short>();
            packet.Decode<byte>();
            
            // if ( v459 && !is_shoot_skill_not_consuming_bullet(nSkillID) )
            // COutPacket::Encode4(&v482, pnItemID);
            
            Entries = new List<AttackInfoEntry>();
            for (var i = 0; i < Count; i++)
            {
                var entry = new AttackInfoEntry();

                entry.Decode(packet, DamagePerMob);
                Entries.Add(entry);
            }

            packet.Decode<short>();
            packet.Decode<short>();
            
            // if Wildhunter, short
            
            packet.Decode<short>();
            packet.Decode<short>();
            
            // if skill == StrikerSpark int, m_tReserveSpark
        }
    }
}