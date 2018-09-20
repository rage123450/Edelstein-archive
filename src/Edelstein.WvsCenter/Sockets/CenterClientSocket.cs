using System;
using DotNetty.Transport.Channels;
using Edelstein.Network;
using Edelstein.Network.Interop;
using Edelstein.Network.Interop.Game;
using Edelstein.Network.Matchers;
using Edelstein.Network.Packets;
using Edelstein.WvsCenter.Logging;
using Lamar;

namespace Edelstein.WvsCenter.Sockets
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
            string serverName;

            this.ServerType = serverType;

            switch (serverType)
            {
                case ServerType.Login:
                    var loginInformation = new LoginInformation();

                    loginInformation.Decode(packet);
                    serverName = loginInformation.Name;
                    break;
                case ServerType.Game:
                    var channelInformation = new ChannelInformation();

                    channelInformation.Decode(packet);
                    this._wvsCenter.WorldInformation.Channels.Add(channelInformation);
                    serverName = channelInformation.Name;
                    break;
                default:
                    using (var p = new OutPacket(InteropSendOperations.RegisterServerResult))
                    {
                        p.Encode<bool>(true);
                        SendPacket(p);
                    }

                    return;
            }

            using (var p = new OutPacket(InteropSendOperations.RegisterServerResult))
            {
                p.Encode<bool>(false);
                this._wvsCenter.WorldInformation.Encode(p);
                SendPacket(p);
            }

            using (var p = new OutPacket(InteropSendOperations.UpdateWorldInformation))
            {
                this._wvsCenter.WorldInformation.Encode(p);
                this._wvsCenter.InteropServer.ChannelGroup.WriteAndFlushAsync(p, new EveryOneBut(Channel.Id));
            }

            Logger.Info($"Registered {Enum.GetName(typeof(ServerType), serverType)} server, {serverName}");
        }
    }
}