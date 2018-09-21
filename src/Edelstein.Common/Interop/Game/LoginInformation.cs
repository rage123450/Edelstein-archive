using Edelstein.Network.Packets;

namespace Edelstein.Common.Interop.Game
{
    public class LoginInformation : IEncodable, IDecodable
    {
        public byte ID { get; set; }
        public string Name { get; set; }

        public void Encode(OutPacket packet)
        {
            packet.Encode<byte>(ID);
            packet.Encode<string>(Name);
        }

        public void Decode(InPacket packet)
        {
            ID = packet.Decode<byte>();
            Name = packet.Decode<string>();
        }
    }
}