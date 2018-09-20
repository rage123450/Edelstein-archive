using Edelstein.Network.Packets;

namespace Edelstein.Network.Interop.Game
{
    public class ChannelInformation : IEncodable, IDecodable
    {
        public byte ID;
        public byte WorldID;
        public string Name;
        public int UserNo;
        public bool AdultChannel;

        public void Encode(OutPacket packet)
        {
            packet.Encode<string>(Name);
            packet.Encode<int>(UserNo);
            packet.Encode<byte>(WorldID);
            packet.Encode<byte>(ID);
            packet.Encode<bool>(AdultChannel);
        }

        public void Decode(InPacket packet)
        {
            Name = packet.Decode<string>();
            UserNo = packet.Decode<int>();
            WorldID = packet.Decode<byte>();
            ID = packet.Decode<byte>();
            AdultChannel = packet.Decode<bool>();
        }
    }
}