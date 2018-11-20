using Edelstein.Network.Packets;
using Edelstein.WvsGame.Fields.Objects.Users;
using Edelstein.WvsGame.Packets;

namespace Edelstein.WvsGame.Interactions
{
    public abstract class Dialogue
    {
        public abstract bool OnPacket(FieldUser user, GameRecvOperations operation, InPacket packet);
        public abstract OutPacket GetCreatePacket();
    }
}