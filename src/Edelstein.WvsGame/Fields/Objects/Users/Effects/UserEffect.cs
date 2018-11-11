using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Fields.Objects.Users.Effects
{
    public class UserEffect : IEncodable
    {
        private readonly UserEffectType _type;
        
        public UserEffect(UserEffectType type)
        {
            _type = type;
        }

        public virtual void Encode(OutPacket packet)
        {
            packet.Encode<byte>((byte) _type);
        }
    }
}