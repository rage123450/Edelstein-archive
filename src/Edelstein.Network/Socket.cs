using System;
using System.Threading.Tasks;
using Edelstein.Network.Packets;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;

namespace Edelstein.Network
{
    public abstract class Socket : IDisposable
    {
        public static readonly AttributeKey<Socket> SocketKey = AttributeKey<Socket>.ValueOf("Socket");

        private readonly IChannel _channel;
        public uint SeqSend { get; set; }
        public uint SeqRecv { get; set; }
        public bool EncryptData { get; set; } = true;

        public readonly object LockSend = new object();
        public readonly object LockRecv = new object();

        public Socket(IChannel channel, uint seqSend, uint seqRecv)
        {
            this._channel = channel;
            this.SeqSend = seqSend;
            this.SeqRecv = seqRecv;
        }

        public abstract void OnPacket(InPacket packet);

        public void Dispose()
        {
            this._channel.CloseAsync();
        }
    }
}