using Edelstein.Network.Packets;
using Edelstein.Provider.NPC;
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