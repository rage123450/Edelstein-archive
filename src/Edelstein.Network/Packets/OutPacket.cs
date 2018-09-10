using DotNetty.Buffers;

namespace Edelstein.Network.Packets
{
    public class OutPacket : Packet
    {
        public OutPacket()
        {
        }

        public OutPacket(IByteBuffer buffer) : base(buffer)
        {
        }

        public virtual OutPacket Encode<T>(T value)
        {
            var type = typeof(T);

            if (PacketMethods.EncodeMethods.ContainsKey(type))
                PacketMethods.EncodeMethods[type](this.Buffer, value);

            return this;
        }
    }
}