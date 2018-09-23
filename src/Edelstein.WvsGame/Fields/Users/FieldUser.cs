using System;
using System.Threading.Tasks;
using Edelstein.Common.Packets;
using Edelstein.Database.Entities;
using Edelstein.Network.Packets;
using Edelstein.WvsGame.Packets;
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
            using (var p = new OutPacket(GameSendOperations.SetField))
            {
                p.Encode<short>(0); // ClientOpt

                p.Encode<int>(0);
                p.Encode<int>(0);

                p.Encode<bool>(true); // sNotifierMessage._m_pStr
                p.Encode<bool>(!Socket.IsInstantiated);
                p.Encode<short>(0); // nNotifierCheck, loops

                if (!Socket.IsInstantiated)
                {
                    p.Encode<int>(0); // seed1
                    p.Encode<int>(0); // seed2
                    p.Encode<int>(0); // seed3

                    Character.EncodeData(p);

                    p.Encode<int>(0);
                    for (var i = 0; i < 3; i++) p.Encode<int>(0);
                }
                else
                {
                    p.Encode<byte>(0);
                    p.Encode<int>(Field.ID);
                    p.Encode<byte>(0);
                    p.Encode<int>(Character.HP);
                    p.Encode<bool>(false);
                }

                p.Encode<long>(0);
                return p;
            }
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