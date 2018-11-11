using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Fields.Objects.Users.Effects
{
    public class UserEffect : IEncodable
    {
        public UserEffectType Type { get; }
        
        public UserEffect(UserEffectType type)
        {
            Type = type;
        }

        public virtual void Encode(OutPacket packet)
        {
            packet.Encode<byte>((byte) Type);
        }
    }
}