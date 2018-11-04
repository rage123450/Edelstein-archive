using Edelstein.Database.Entities;
using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Fields.Attacking
{
    public class MagicAttackInfo : AttackInfo
    {
        public MagicAttackInfo(Character character) : base(character)
        {
        }

        public override void Decode(InPacket packet)
        {
            throw new System.NotImplementedException();
        }

        public override void Encode(OutPacket packet)
        {
            throw new System.NotImplementedException();
        }
    }
}