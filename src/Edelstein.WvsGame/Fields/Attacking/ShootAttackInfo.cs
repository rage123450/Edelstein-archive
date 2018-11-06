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
            throw new System.NotImplementedException();
        }
    }
}