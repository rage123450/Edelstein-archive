using System;
using DotNetty.Transport.Channels;
using Edelstein.Network;
using Edelstein.Network.Interop;
using Edelstein.Network.Packets;
using Edelstein.WvsCenter.Logging;
using Lamar;

namespace Edelstein.WvsCenter
{
    public class CenterClientSocket : Socket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private WvsCenter _wvsCenter;
        private WvsCenterOptions _options;

        public ServerType ServerType { get; set; } = ServerType.Undefined;
        public string ServerName { get; set; }

        public CenterClientSocket(IContainer container, IChannel channel, uint seqSend, uint seqRecv)
            : base(channel, seqSend, seqRecv)
        {
            this._wvsCenter = container.GetInstance<WvsCenter>();
            this._options = container.GetInstance<WvsCenterOptions>();
        }

        public override void OnPacket(InPacket packet)
        {
            var operation = (InteropRecvOperations) packet.Decode<short>();

            switch (operation)
            {
                case InteropRecvOperations.RegisterServer:
                    this.OnRegisterServer(packet);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnRegisterServer(InPacket packet)
        {
            var serverType = (ServerType) packet.Decode<byte>();

            if (!Enum.IsDefined(typeof(ServerType), serverType)
                && serverType != ServerType.Undefined
                || this.ServerType != ServerType.Undefined)
            {
                using (var p = new OutPacket(InteropSendOperations.RegisterServerResult))
                {
                    p.Encode<byte>(1);
                    SendPacket(p);
                }
                return;
            }

            this.ServerType = serverType;
            this.ServerName = packet.Decode<string>();
            Logger.Info($"Registered {Enum.GetName(typeof(ServerType), serverType)} server, {ServerName}");
        }
    }
}