using DotNetty.Buffers;

namespace Edelstein.Network.Packets
{
    public class InPacket : Packet
    {
        public InPacket()
        {
        }

        public InPacket(IByteBuffer buffer) : base(buffer)
        {
        }

        public virtual T Decode<T>()
        {
            var type = typeof(T);

            if (PacketMethods.DecodeMethods.ContainsKey(type))
                return (T) PacketMethods.DecodeMethods[type](this.Buffer);

            return default(T);
        }
    }
}