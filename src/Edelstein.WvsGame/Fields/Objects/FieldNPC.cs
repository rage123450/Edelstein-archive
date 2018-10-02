using System;
using Edelstein.Network.Packets;
using Edelstein.Provider.NPC;
using Edelstein.WvsGame.Fields.Movements;
using Edelstein.WvsGame.Fields.Objects.Users;
using Edelstein.WvsGame.Packets;

namespace Edelstein.WvsGame.Fields.Objects
{
    public class FieldNPC : FieldObjectControlled
    {
        public NPCTemplate Template { get; set; }

        public int RX0 { get; set; }
        public int RX1 { get; set; }

        public FieldNPC(NPCTemplate template)
        {
            Template = template;
        }

        public bool OnPacket(FieldUser controller, GameRecvOperations operation, InPacket packet)
        {
            switch (operation)
            {
                case GameRecvOperations.NpcMove:
                    OnNpcMove(packet);
                    break;
                default:
                    return false;
            }

            return true;
        }

        private void OnNpcMove(InPacket packet)
        {
            using (var p = new OutPacket(GameSendOperations.NpcMove))
            {
                p.Encode<int>(ID);
                p.Encode<byte>(packet.Decode<byte>());
                p.Encode<byte>(packet.Decode<byte>());

                if (packet.Length > 0) // m_pTemplate->bMove
                {
                    var movementPath = new MovementPath();

                    movementPath.Decode(packet);

                    X = movementPath.X;
                    Y = movementPath.Y;
                    MoveAction = movementPath.MoveActionLast;
                    Foothold = movementPath.FHLast;
                    movementPath.Encode(p);
                }

                Field.BroadcastPacket(p);
            }
        }

        public override OutPacket GetEnterFieldPacket()
        {
            using (var p = new OutPacket(GameSendOperations.NpcEnterField))
            {
                p.Encode<int>(ID);
                p.Encode<int>(Template.TemplateID);

                p.Encode<short>(X);
                p.Encode<short>(Y);
                p.Encode<byte>(MoveAction);
                p.Encode<short>(Foothold);

                p.Encode<short>((short) RX0);
                p.Encode<short>((short) RX1);

                p.Encode<bool>(true); // bEnabled
                return p;
            }
        }

        public override OutPacket GetLeaveFieldPacket()
        {
            using (var p = new OutPacket(GameSendOperations.NpcLeaveField))
            {
                p.Encode<int>(ID);
                return p;
            }
        }

        public override OutPacket GetChangeControllerPacket(bool setAsController)
        {
            using (var p = new OutPacket(GameSendOperations.NpcChangeController))
            {
                p.Encode<bool>(setAsController);
                p.Encode<int>(ID);
                return p;
            }
        }
    }
}