namespace Edelstein.Network.Packets
{
    public interface IEncodable
    {
        void Encode(OutPacket packet);
    }
}