using DotNetty.Transport.Channels;
using Edelstein.Network.Crypto;
using Edelstein.Network.Logging;
using Edelstein.Network.Packets;

namespace Edelstein.Network
{
    public class ClientAdapter<T> : ChannelHandlerAdapter
        where T : Socket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly Client<T> _client;
        private readonly ISocketFactory<T> _socketFactory;

        public ClientAdapter(
            Client<T> client,
            ISocketFactory<T> socketFactory
        )
        {
            _client = client;
            _socketFactory = socketFactory;
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var socket = (T) context.Channel.GetAttribute(Socket.SocketKey).Get();
            var p = (InPacket) message;

            if (socket != null) socket.OnPacket(p);
            else
            {
                var version = p.Decode<short>();

                p.Decode<string>();

                var seqSend = p.Decode<uint>();
                var seqRecv = p.Decode<uint>();

                p.Decode<byte>();

                if (version != AESCipher.Version) return;

                var newSocket = _socketFactory.Build(
                    context.Channel,
                    seqSend,
                    seqRecv
                );

                lock (_client)
                {
                    if (_client.Socket == null)
                    {
                        _client.Socket = newSocket;
                    }
                }

                context.Channel.GetAttribute(Socket.SocketKey).Set(newSocket);
            }
        }
    }
}