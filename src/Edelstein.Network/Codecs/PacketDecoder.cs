using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Edelstein.Network.Crypto;
using Edelstein.Network.Packets;

namespace Edelstein.Network.Codecs
{
    public class PacketDecoder : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            var socket = context.Channel.GetAttribute(Socket.SocketKey).Get();

            if (socket == null) return;

            lock (socket.LockRecv)
            {
                var length = 0;
                var seqRecv = socket.SeqRecv;

                if (input.ReadableBytes >= 4)
                {
                    var dataLen = input.ReadShortLE();
                    var rawSeq = input.ReadShortLE();

                    if (socket.EncryptData) dataLen ^= rawSeq;
                    //if (((seqRecv >> 16) ^ rawSeq) != AESCipher.Version) return;
                    if (dataLen > 0x10000) return;

                    length = dataLen;
                }

                if (length < 2) return;
                if (input.ReadableBytes < length) return;

                var buffer = new byte[length];

                input.ReadBytes(buffer);

                if (socket.EncryptData)
                {
                    buffer = AESCipher.Transform(buffer, seqRecv);
                    buffer = ShandaCipher.DecryptTransform(buffer);
                }

                socket.SeqRecv = CIGCipher.InnoHash(seqRecv, 4, 0);
                output.Add(new InPacket(Unpooled.CopiedBuffer(buffer)));
            }
        }
    }
}