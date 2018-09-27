using Edelstein.Network.Packets;

namespace Edelstein.Common.Packets.Inventory.Operations
{
    public class InventoryUpdateEXPOperation : InventoryOperation
    {
        private readonly int _exp;

        public InventoryUpdateEXPOperation(ModifyInventoryType inventory, short slot, int EXP)
            : base(InventoryOperationType.UpdateEXP, inventory, slot)
        {
            _exp = EXP;
        }

        public override void Encode(OutPacket packet)
        {
            base.Encode(packet);

            packet.Encode<int>(_exp);
        }
    }
}