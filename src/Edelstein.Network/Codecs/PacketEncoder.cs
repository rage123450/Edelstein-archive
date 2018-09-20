using System;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Edelstein.Network.Crypto;
using Edelstein.Network.Packets;

namespace Edelstein.Network.Codecs
{
    public class PacketEncoder : MessageToByteEncoder<Packet>
    {
        protected override void Encode(IChannelHandlerContext context, Packet message, IByteBuffer output)
        {
            var socket = context.Channel.GetAttribute(Socket.SocketKey).Get();

            if (socket != null)
            {
                lock (socket.LockSend)
                {
                    var seqSend = socket.SeqSend;
                    var rawSeq = (short) ((seqSend >> 16) ^ -(AESCipher.Version + 1));
                    var dataLen = (short) message.Length;
                    var buffer = new byte[dataLen];

                    message.Buffer.ReadBytes(buffer);

                    if (socket.EncryptData)
                    {
                        dataLen ^= rawSeq;

                        buffer = ShandaCipher.EncryptTransform(buffer);
                        buffer = AESCipher.Transform(buffer, seqSend);
                    }

                    output.WriteShortLE(rawSeq);
                    output.WriteShortLE(dataLen);
                    output.WriteBytes(buffer);

                    socket.SeqSend = CIGCipher.InnoHash(seqSend, 4, 0);
                }
            }
            else
            {
                output.WriteBytes(message.Buffer);
            }
        }
    }
}