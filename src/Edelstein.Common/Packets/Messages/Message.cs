using Edelstein.Network.Packets;

namespace Edelstein.Common.Packets.Messages
{
    public abstract class Message : IEncodable
    {
        protected abstract byte Type { get; }
        
        public virtual void Encode(OutPacket packet)
        {
            packet.Encode<byte>(Type);
        }
    }
}