using System.Drawing;
using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Fields
{
    public abstract class FieldObj
    {
        public abstract FieldObjType Type { get; }
        public int ID { get; set; }
        public Field Field { get; set; }

        public Point Position { get; set; }

        public abstract OutPacket GetEnterFieldPacket();
        public abstract OutPacket GetLeaveFieldPacket();
    }
}