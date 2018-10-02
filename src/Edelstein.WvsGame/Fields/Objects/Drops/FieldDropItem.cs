using Edelstein.Database.Entities.Inventory;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Fields.Objects.Drops
{
    public class FieldDropItem : FieldDrop
    {
        public ItemSlot Item { get; set; }

        public override bool IsMoney => false;
        public override int Info => Item.TemplateID;

        public FieldDropItem(ItemSlot item)
        {
            Item = item;
        }

        public override void PickUp(FieldUser user)
        {
            user.ModifyInventory(i => i.Add(Item), true);
        }
    }
}