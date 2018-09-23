using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Fields
{
    public abstract class FieldObject
    {
        public int ID { get; set; }
        public Field Field { get; set; }

        public abstract OutPacket GetEnterFieldPacket();
        public abstract OutPacket GetLeaveFieldPacket();
    }
}