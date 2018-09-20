namespace Edelstein.Network.Packets
{
    public interface IDecodable
    {
        void Decode(InPacket packet);
    }
}