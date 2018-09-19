using System;
using DotNetty.Transport.Channels;
using Edelstein.Network;
using Edelstein.Network.Interop;
using Edelstein.Network.Packets;
using Edelstein.WvsCenter.Logging;

namespace Edelstein.WvsCenter
{
    public class CenterClientSocket : Socket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public ServerType ServerType { get; set; } = ServerType.Undefined;
        public string ServerName { get; set; }

        public CenterClientSocket(IChannel channel, uint seqSend, uint seqRecv)
            : base(channel, seqSend, seqRecv)
        {
        }

        public override void OnPacket(InPacket packet)
        {
            var operation = (InteropRecvOperations) packet.Decode<short>();

            switch (operation)
            {
                case InteropRecvOperations.RegisterServer:
                    var serverType = (ServerType) packet.Decode<byte>();

                    if (!Enum.IsDefined(typeof(ServerType), serverType)
                        && serverType != ServerType.Undefined)
                    {
                        return;
                    }

                    if (this.ServerType != ServerType.Undefined)
                    {
                        return;
                    }

                    this.ServerType = serverType;
                    this.ServerName = packet.Decode<string>();
                    Logger.Info($"Registered {Enum.GetName(typeof(ServerType), serverType)} server, {ServerName}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}