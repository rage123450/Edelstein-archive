using System.Net;
using System.Threading.Tasks;
using Edelstein.Network.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using DotNetty.Transport.Channels.Sockets;

namespace Edelstein.Network
{
    public class Server<T> : ChannelHandlerAdapter
        where T : Socket
    {
        public IChannel Channel { get; private set; }
        public IChannelGroup ChannelGroup { get; set; }
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
                        new ServerAdapter<T>(this, this._socketFactory),
                        new PacketEncoder()
                    );
                }))
                .BindAsync(IPAddress.Parse(this._options.Host), this._options.Port);
        }
    }
}