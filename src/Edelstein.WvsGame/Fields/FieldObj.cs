using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Fields
{
    public abstract class FieldObj
    {
        public int ID { get; set; }
        public Field Field { get; set; }

        public short X { get; set; }
        public short Y { get; set; }

        public abstract OutPacket GetEnterFieldPacket();
        public abstract OutPacket GetLeaveFieldPacket();
    }
}