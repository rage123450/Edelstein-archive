using System;
using System.Net;
using System.Threading.Tasks;
using Edelstein.Network.Codecs;
using Edelstein.Network.Crypto;
using Edelstein.Network.Packets;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace Edelstein.Network
{
    public class Server<T> : ChannelHandlerAdapter
        where T : Socket
    {
        public IChannel Channel { get; private set; }
        private readonly ServerOptions _options;
        private readonly ISocketFactory<T> _socketFactory;

        public Server(
            ServerOptions options,
            ISocketFactory<T> socketFactory
        )
        {
            this._options = options;
            this._socketFactory = socketFactory;
        }

        public async Task Run()
        {
            var bossGroup = new MultithreadEventLoopGroup();
            var workerGroup = new MultithreadEventLoopGroup();

            this.Channel = await new ServerBootstrap()
                .Group(bossGroup, workerGroup)
                .Channel<TcpServerSocketChannel>()
                .Option(ChannelOption.SoBacklog, 1024)
                .ChildHandler(new ActionChannelInitializer<IChannel>(ch =>
                {
                    ch.Pipeline.AddLast(
                        new PacketDecoder(),
                        this,
                        new PacketEncoder()
                    );
                }))
                .BindAsync(IPAddress.Parse(this._options.Host), this._options.Port);
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            var random = new Random();
            var socket = this._socketFactory.Build(
                context.Channel,
                (uint) random.Next(),
                (uint) random.Next()
            );

            using (var p = new OutPacket())
            {
                p.Encode<short>(0x0E);
                p.Encode<short>(AESCipher.Version);
                p.Encode<string>("1");
                p.Encode<uint>(socket.SeqRecv);
                p.Encode<uint>( socket.SeqSend);
                p.Encode<byte>(8);

                socket.SendPacket(p);
            }

            context.Channel.GetAttribute(Socket.SocketKey).Set(socket);
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var socket = (T) context.Channel.GetAttribute(Socket.SocketKey).Get();
            var p = (InPacket) message;

            socket?.OnPacket(p);
        }
    }
}