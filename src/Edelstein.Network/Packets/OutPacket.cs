using System;
using DotNetty.Buffers;

namespace Edelstein.Network.Packets
{
    public class OutPacket : Packet
    {
        public OutPacket()
        {
        }

        public OutPacket(Enum operation)
        {
            Encode<short>(Convert.ToInt16(operation));
        }

        public OutPacket(IByteBuffer buffer) : base(buffer)
        {
        }

        public virtual OutPacket Encode<T>(T value)
        {
            var type = typeof(T);

            if (value == null) value = default(T);

            if (PacketMethods.EncodeMethods.ContainsKey(type))
                PacketMethods.EncodeMethods[type](this.Buffer, value);

            return this;
        }

        public virtual OutPacket EncodeFixedString(string value, int length)
        {
            for (var i = 0; i < length; i++)
            {
                if (i < value.Length) Encode<byte>((byte) value[i]);
                else Encode<byte>(0);
            }

            return this;
        }
    }
}