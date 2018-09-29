using Edelstein.Network.Packets;
using Edelstein.WvsGame.Packets;

namespace Edelstein.WvsGame.Fields.Objects.Drops
{
    public abstract class FieldDrop : FieldObject
    {
        public abstract bool IsMoney { get; }
        public abstract int Info { get; }

        public override OutPacket GetEnterFieldPacket()
        {
            using (var p = new OutPacket(GameSendOperations.DropEnterField))
            {
                p.Encode<byte>(2); // nEnterType
                p.Encode<int>(ID); // nZMass

                p.Encode<bool>(IsMoney);
                p.Encode<int>(Info);
                p.Encode<int>(0); // dwOwnerID
                p.Encode<byte>(0); // nOwnType
                p.Encode<short>(X);
                p.Encode<short>(Y);
                p.Encode<int>(0); // dwSourceID

                if (!IsMoney)
                    p.Encode<long>(0);

                p.Encode<bool>(false);
                p.Encode<bool>(false);
                return p;
            }
        }

        public override OutPacket GetLeaveFieldPacket()
        {
            using (var p = new OutPacket(GameSendOperations.DropLeaveField))
            {
                p.Encode<byte>(0); // nLeaveType
                p.Encode<int>(ID);
                return p;
            }
        }

        public abstract void PickUp(FieldUser user);
    }
}