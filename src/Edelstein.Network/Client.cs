using System.Net;
using System.Threading.Tasks;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Edelstein.Network.Codecs;
using Edelstein.Network.Packets;

namespace Edelstein.Network
{
    public class Client<T>
        where T : Socket
    {
        public IEventLoopGroup WorkerGroup { get; set; }
        public IChannel Channel { get; private set; }
        public T Socket { get; set; }
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
            WorkerGroup = new MultithreadEventLoopGroup();
            this.Channel = await new Bootstrap()
                .Group(WorkerGroup)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Handler(new ActionChannelInitializer<IChannel>(ch =>
                {
                    ch.Pipeline.AddLast(
                        new PacketDecoder(),
                        new ClientAdapter<T>(this, this._socketFactory),
                        new PacketEncoder()
                    );
                }))
                .ConnectAsync(IPAddress.Parse(this._options.TargetHost), this._options.TargetPort);
        }

        public Task SendPacket(OutPacket packet) => this.Socket.SendPacket(packet);
    }
}