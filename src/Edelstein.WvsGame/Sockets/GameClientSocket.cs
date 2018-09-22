using System;
using DotNetty.Transport.Channels;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.WvsGame.Logging;
using Lamar;

namespace Edelstein.WvsGame.Sockets
{
    public class GameClientSocket : Socket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        private IContainer _container;
        private WvsGame _wvsGame;

        public GameClientSocket(IContainer container, IChannel channel, uint seqSend, uint seqRecv)
            : base(channel, seqSend, seqRecv)
        {
            this._container = container;
            this._wvsGame = container.GetInstance<WvsGame>();
        }

        public override void OnPacket(InPacket packet)
        {
            var operation = packet.Decode<short>();

            Console.WriteLine(operation);
        }

        public override void OnDisconnect()
        {
            throw new System.NotImplementedException();
        }
    }
}