namespace Edelstein.Network.Packets
{
    public interface IPacketHandler<in T>
        where T : Socket
    {
        void handle(T socket);
    }
}