using System.Collections.Generic;
using Edelstein.Database.Entities;
using Edelstein.Network.Packets;
using MoreLinq.Extensions;

namespace Edelstein.WvsGame.Fields.Attacking
{
    public class MeleeAttackInfo : AttackInfo
    {
        public MeleeAttackInfo(Character character) : base(character)
        {
        }

        public override void Decode(InPacket packet)
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
            packet.Decode<byte>();
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

        public override void Encode(OutPacket packet)
        {
            packet.Encode<byte>((byte) (DamagePerMob | 16 * Count));
            packet.Encode<byte>(Character.Level);

            var skillLevel = (byte) (SkillID > 0 ? 1 : 0); // TODO: hacky
            packet.Encode<byte>(skillLevel);
            if (skillLevel > 0)
                packet.Encode<int>(SkillID);

            packet.Encode<byte>(0x20); // bSerialAttack
            packet.Encode<short>((short) (AttackAction & 0x7FFF | ((IsLeft ? 1 : 0) << 15)));

            if (AttackAction > 0x110) return;
            packet.Encode<byte>(0); // nMastery
            packet.Encode<byte>(0); // v82
            packet.Encode<int>(0); // bMovingShoot

            Entries.ForEach(e =>
            {
                packet.Encode<int>(e.MobID);

                if (e.MobID <= 0) return;

                packet.Encode<byte>(e.HitAction);

                // check 4211006

                e.Damage.ForEach(d =>
                {
                    packet.Encode<bool>(false);
                    packet.Encode<int>(d);
                });
            });
        }
    }
}