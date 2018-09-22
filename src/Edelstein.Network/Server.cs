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
        public IEventLoopGroup bossGroup { get; set; }
        public IEventLoopGroup workerGroup { get; set; }
        public IChannel Channel { get; private set; }
        public ICollection<T> Sockets { get; set; }

        private readonly ServerOptions _options;
        private readonly ISocketFactory<T> _socketFactory;

        public Server(
            ServerOptions options,
            ISocketFactory<T> socketFactory
        )
        {
            this.Sockets = new List<T>();
            this._options = options;
            this._socketFactory = socketFactory;
        }

        public async Task Run()
        {
            bossGroup = new MultithreadEventLoopGroup();
            workerGroup = new MultithreadEventLoopGroup();

            this.Channel = await new ServerBootstrap()
                .Group(bossGroup, workerGroup)
                .Channel<TcpServerSocketChannel>()
                .Option(ChannelOption.SoBacklog, 1024)
                .ChildHandler(new ActionChannelInitializer<IChannel>(ch =>
                {
                    ch.Pipeline.AddLast(
                        new PacketDecoder(),
                        new ServerAdapter<T>(this, this._socketFactory),
                        new PacketEncoder()
                    );
                }))
                .BindAsync(IPAddress.Parse(this._options.Host), this._options.Port);
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