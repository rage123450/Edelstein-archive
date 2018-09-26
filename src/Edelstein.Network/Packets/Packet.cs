using System;
using DotNetty.Buffers;

namespace Edelstein.Network.Packets
{
    public class Packet : IDisposable
    {
        public IByteBuffer Buffer { get; }
        public int Length => Buffer.ReadableBytes;

        public Packet() : this(Unpooled.Buffer())
        {
        }

        public Packet(IByteBuffer byteBuffer)
        {
            Buffer = byteBuffer;
        }

        public void Dispose()
        {
            Buffer.DiscardReadBytes();
        }
    }
}