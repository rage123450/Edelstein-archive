using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Interactions.Dialogue
{
    public interface Dialogue
    {
        OutPacket GetCreatePacket();
    }
}