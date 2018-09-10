using System;
using System.Collections.Generic;
using Edelstein.Network.Codecs;
using Edelstein.Network.Crypto;
using Edelstein.Network.Packets;
using DotNetty.Common.Concurrency;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace Edelstein.Network
{
    public class Server<T> : ChannelHandlerAdapter, IRunnable
        where T : Socket
    {
        public IChannel Channel { get; private set; }
        private readonly ISocketFactory<T> _socketFactory;
        private readonly Dictionary<short, IPacketHandler<T>> _handlers;

        public Server(
            ISocketFactory<T> socketFactory,
            Dictionary<short, IPacketHandler<T>> handlers
        )
        {
            this._socketFactory = socketFactory;
            this._handlers = handlers;
        }

        public async void Run()
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
                .BindAsync(8484);
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            var random = new Random();
            var socket = this._socketFactory.createNew(
                context.Channel,
                (uint) random.Next(),
                (uint) random.Next()
            );

            using (var p = new OutPacket())
            {
                p.Encode<short>(0x0E);
                p.Encode<short>(AESCipher.Version);
                p.Encode<string>("1");
                p.Encode<int>((int) socket.SeqRecv);
                p.Encode<int>((int) socket.SeqSend);
                p.Encode<byte>(8);

                socket.SendPacket(p);
            }

            context.Channel.GetAttribute(Socket.SocketKey).Set(socket);
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var socket = (T) context.Channel.GetAttribute(Socket.SocketKey).Get();
            var p = (InPacket) message;

            if (socket == null) return;

            var operation = p.Decode<short>();
            var handler = this._handlers[operation];

            if (handler != null) handler.handle(socket);
            else Console.WriteLine("Unhandled: " + operation);
        }
    }
}