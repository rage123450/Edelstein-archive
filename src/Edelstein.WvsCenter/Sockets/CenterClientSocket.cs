using System;
using System.Linq;
using DotNetty.Transport.Channels;
using Edelstein.Common.Interop;
using Edelstein.Common.Interop.Game;
using Edelstein.Network;
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
        public byte ServerID { get; set; }

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
                case InteropRecvOperations.ServerRegister:
                    this.OnRegisterServer(packet);
                    break;
                case InteropRecvOperations.MigrationRequest:
                    this.OnMigrationRequest(packet);
                    break;
                case InteropRecvOperations.MigrationRegisterResult:
                    this.OnMigrationRegisterResult(packet);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void OnDisconnect()
        {
            if (ServerType == ServerType.Undefined) return;

            switch (ServerType)
            {
                case ServerType.Game:
                    var channelInformation =
                        this._wvsCenter.WorldInformation.Channels.SingleOrDefault(c => c.ID == ServerID);

                    if (channelInformation != null)
                    {
                        this._wvsCenter.WorldInformation.Channels.Remove(channelInformation);
                        Logger.Info(
                            $"Removed {Enum.GetName(typeof(ServerType), ServerType)} server, {channelInformation.Name}");
                    }

                    break;
            }

            using (var p = new OutPacket(InteropSendOperations.ServerInformation))
            {
                this._wvsCenter.WorldInformation.Encode(p);
                this._wvsCenter.InteropServer.BroadcastPacket(p, new EveryOneBut(Channel.Id));
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
                    ServerID = loginInformation.ID;
                    serverName = loginInformation.Name;
                    break;
                case ServerType.Game:
                    var channelInformation = new ChannelInformation();

                    channelInformation.Decode(packet);
                    this._wvsCenter.WorldInformation.Channels.Add(channelInformation);
                    ServerID = channelInformation.ID;
                    serverName = channelInformation.Name;
                    break;
                default:
                    using (var p = new OutPacket(InteropSendOperations.ServerRegisterResult))
                    {
                        p.Encode<bool>(true);
                        SendPacket(p);
                    }

                    return;
            }

            using (var p = new OutPacket(InteropSendOperations.ServerRegisterResult))
            {
                p.Encode<bool>(false);
                this._wvsCenter.WorldInformation.Encode(p);
                SendPacket(p);
            }

            using (var p = new OutPacket(InteropSendOperations.ServerInformation))
            {
                this._wvsCenter.WorldInformation.Encode(p);
                this._wvsCenter.InteropServer.BroadcastPacket(p, new EveryOneBut(Channel.Id));
            }

            Logger.Info($"Registered {Enum.GetName(typeof(ServerType), serverType)} server, {serverName}");
        }

        private void OnMigrationRequest(InPacket packet)
        {
            var serverType = (ServerType) packet.Decode<byte>();
            var serverID = packet.Decode<byte>();

            var server = _wvsCenter.InteropServer.Sockets
                .Single(s => s.ServerType == serverType &&
                             s.ServerID == serverID);

            using (var p = new OutPacket(InteropSendOperations.MigrationRegistryRequest))
            {
                p.Encode<string>(SessionKey);
                p.Encode<string>(packet.Decode<string>());
                p.Encode<int>(packet.Decode<int>());
                server.SendPacket(p);
            }
        }

        private void OnMigrationRegisterResult(InPacket packet)
        {
            var sessionKey = packet.Decode<string>();
            var server = _wvsCenter.InteropServer.Sockets.Single(s => s.SessionKey == sessionKey);

            using (var p = new OutPacket(InteropSendOperations.MigrationResult))
            {
                p.Encode<string>(packet.Decode<string>());

                var result = packet.Decode<bool>();

                p.Encode<bool>(result);

                if (!result) return;

                p.Encode<int>(packet.Decode<int>());

                p.Encode<byte>(192);
                p.Encode<byte>(168);
                p.Encode<byte>(2);
                p.Encode<byte>(250);
                p.Encode<short>(7575);

                server.SendPacket(p);

                Console.WriteLine(server.ServerType);
            }
        }
    }
}