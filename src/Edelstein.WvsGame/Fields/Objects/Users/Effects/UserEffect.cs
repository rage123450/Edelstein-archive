using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Fields.Objects.Users.Effects
{
    public abstract class UserEffect : IEncodable
    {
        public abstract UserEffectType Type { get; }

        public virtual void Encode(OutPacket packet)
        {
            packet.Encode<byte>((byte) Type);
        }
    }
}