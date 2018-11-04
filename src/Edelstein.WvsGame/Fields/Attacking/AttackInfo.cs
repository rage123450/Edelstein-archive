using System.Collections.Generic;
using Edelstein.Database.Entities;
using Edelstein.Network.Packets;

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
        public abstract void Encode(OutPacket packet);
    }
}