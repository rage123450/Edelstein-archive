using Edelstein.Network.Packets;

namespace Edelstein.Network.Interop.Game
{
    public class LoginInformation : IEncodable, IDecodable
    {
        public string Name { get; set; }

        public void Encode(OutPacket packet)
        {
            packet.Encode<string>(Name);
        }

        public void Decode(InPacket packet)
        {
            Name = packet.Decode<string>();
        }
    }
}