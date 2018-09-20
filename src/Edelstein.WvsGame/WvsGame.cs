using System.Threading.Tasks;
using Edelstein.Network;
using Edelstein.Network.Interop;
using Edelstein.Network.Interop.Game;
using Edelstein.Network.Packets;
using Edelstein.WvsGame.Logging;
using Edelstein.WvsGame.Sockets;
using Lamar;

namespace Edelstein.WvsGame
{
    public class WvsGame
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly IContainer _container;
        public Client<CenterServerSocket> InteropClient;

        public ChannelInformation ChannelInformation { get; set; }

        public WvsGame(IContainer container)
        {
            this._container = container;
        }

        public async Task Run()
        {
            var options = this._container.GetInstance<WvsGameOptions>();
            var info = options.GameInfo;

            this.ChannelInformation = new ChannelInformation
            {
                ID = info.ID,
                Name = info.Name,
                UserNo = 0,
                AdultChannel = info.AdultChannel
            };

            this.InteropClient = new Client<CenterServerSocket>(
                options.InteropClientOptions,
                this._container.GetInstance<CenterServerSocketFactory>()
            );

            await this.InteropClient.Run();
            Logger.Info($"Connected to interoperability server on {this.InteropClient.Channel.RemoteAddress}");

            using (var p = new OutPacket(InteropRecvOperations.RegisterServer))
            {
                p.Encode<byte>((byte) ServerType.Game);
                p.Encode<string>(options.GameInfo.Name);

                await this.InteropClient.Socket.SendPacket(p);
            }
        }
    }
}