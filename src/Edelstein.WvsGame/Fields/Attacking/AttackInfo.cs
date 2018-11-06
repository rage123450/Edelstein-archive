using System.Collections.Generic;
using Edelstein.Common.Utils.Extensions;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Types;
using Edelstein.Network.Packets;
using MoreLinq.Extensions;

namespace Edelstein.WvsGame.Fields.Attacking
{
    public abstract class AttackInfo : IDecodable, IEncodable
    {
        public Character Character { get; set; }

        public int DamagePerMob { get; set; }
        public int Count { get; set; }
        public int SkillID { get; set; }
        public int KeyDown { get; set; }
        public short AttackAction { get; set; }
        public bool IsLeft { get; set; }
        public byte AttackActionType { get; set; }
        public byte AttackSpeed { get; set; }
        public int AttackTime { get; set; }

        public ICollection<AttackInfoEntry> Entries { get; set; }

        public AttackInfo(Character character)
        {
            Character = character;
        }

        public abstract void Decode(InPacket packet);

        public virtual void Encode(OutPacket packet)
        {
            packet.Encode<byte>((byte) (DamagePerMob | 16 * Count));
            packet.Encode<byte>(Character.Level);

            var skillLevel = (byte) (SkillID > 0 ? Character.GetSkillLevel((Skill) SkillID) : 0);
            packet.Encode<byte>(skillLevel);
            if (skillLevel > 0)
                packet.Encode<int>(SkillID);

            packet.Encode<byte>(0x20); // bSerialAttack
            packet.Encode<short>((short) (AttackAction & 0x7FFF | ((IsLeft ? 1 : 0) << 15)));

            if (AttackAction > 0x110) return;
            packet.Encode<byte>(0); // nMastery
            packet.Encode<byte>(0); // v82
            packet.Encode<int>(2070000); // bMovingShoot

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