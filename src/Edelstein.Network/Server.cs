using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Edelstein.Network.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using DotNetty.Transport.Channels.Sockets;
using Edelstein.Network.Packets;

namespace Edelstein.Network
{
    public class Server<T> : ChannelHandlerAdapter
        where T : Socket
    {
        public IEventLoopGroup BossGroup { get; set; }
        public IEventLoopGroup WorkerGroup { get; set; }
        public IChannel Channel { get; private set; }
        public ICollection<T> Sockets { get; set; }

        private readonly ServerOptions _options;
        private readonly ISocketFactory<T> _socketFactory;

        public Server(
            ServerOptions options,
            ISocketFactory<T> socketFactory
        )
        {
            Sockets = new List<T>();
            _options = options;
            _socketFactory = socketFactory;
        }

        public async Task Run()
        {
            BossGroup = new MultithreadEventLoopGroup();
            WorkerGroup = new MultithreadEventLoopGroup();

            Channel = await new ServerBootstrap()
                .Group(BossGroup, WorkerGroup)
                .Channel<TcpServerSocketChannel>()
                .Option(ChannelOption.SoBacklog, 1024)
                .ChildHandler(new ActionChannelInitializer<IChannel>(ch =>
                {
                    ch.Pipeline.AddLast(
                        new PacketDecoder(),
                        new ServerAdapter<T>(this, _socketFactory),
                        new PacketEncoder()
                    );
                }))
                .BindAsync(IPAddress.Parse(_options.Host), _options.Port);
        }


        public Task BroadcastPacket(OutPacket packet)
        {
            return Task.WhenAll(Sockets
                .Select(s => s.SendPacket(packet))
            );
        }

        public Task BroadcastPacket(OutPacket packet, IChannelMatcher matcher)
        {
            return Task.WhenAll(Sockets
                .Where(s => matcher.Matches(s.Channel))
                .Select(s => s.SendPacket(packet))
            );
        }
    }
}