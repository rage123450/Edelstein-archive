using System;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Edelstein.Network.Codecs;
using Edelstein.Network.Crypto;
using Edelstein.Network.Packets;

namespace Edelstein.Network
{
    public class Client<T> : ChannelHandlerAdapter
        where T : Socket
    {
        public IChannel Channel { get; private set; }
        private readonly ClientOptions _options;
        private readonly ISocketFactory<T> _socketFactory;

        public Client(
            ClientOptions options,
            ISocketFactory<T> socketFactory
        )
        {
            this._options = options;
            this._socketFactory = socketFactory;
        }

        public async Task Run()
        {
            var worker = new MultithreadEventLoopGroup();
            this.Channel = await new Bootstrap()
                .Group(worker)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Handler(new ActionChannelInitializer<IChannel>(ch =>
                {
                    ch.Pipeline.AddLast(
                        new PacketDecoder(),
                        this,
                        new PacketEncoder()
                    );
                }))
                .ConnectAsync(IPAddress.Parse(this._options.TargetHost), this._options.TargetPort);
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var socket = (T) context.Channel.GetAttribute(Socket.SocketKey).Get();
            var p = (InPacket) message;

            if (socket != null) socket.OnPacket(p);
            else
            {
                p.Decode<short>();

                var version = p.Decode<short>();

                p.Decode<string>();

                var seqSend = (uint) p.Decode<int>();
                var seqRecv = (uint) p.Decode<int>();

                p.Decode<byte>();

                if (version != AESCipher.Version) return;
                
                var newSocket = _socketFactory.createNew(
                    context.Channel,
                    seqSend,
                    seqRecv
                );

                context.Channel.GetAttribute(Socket.SocketKey).Set(newSocket);
            }
        }
    }
}