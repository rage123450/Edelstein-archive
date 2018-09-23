using System.Threading.Tasks;
using Edelstein.Database.Entities;
using Edelstein.Network.Packets;
using Edelstein.WvsGame.Sockets;

namespace Edelstein.WvsGame.Fields.Users
{
    public class FieldUser : FieldObject
    {
        public GameClientSocket Socket { get; set; }
        public Character Character { get; set; }

        public FieldUser(GameClientSocket socket, Character character)
        {
            Socket = socket;
            Character = character;
        }

        public OutPacket GetSetFieldPacket()
        {
            throw new System.NotImplementedException();
        }

        public override OutPacket GetEnterFieldPacket()
        {
            throw new System.NotImplementedException();
        }

        public override OutPacket GetLeaveFieldPacket()
        {
            throw new System.NotImplementedException();
        }

        public Task SendPacket(OutPacket packet) => this.Socket.SendPacket(packet);
    }
}