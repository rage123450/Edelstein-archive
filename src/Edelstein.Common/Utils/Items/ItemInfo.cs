using Edelstein.Database.Entities.Inventory;
using Edelstein.Provider.Items;

namespace Edelstein.Common.Utils.Items
{
    public static class ItemInfo
    {
        public static ItemSlotEquip FromTemplate(ItemEquipTemplate template,
            ItemVariationType type = ItemVariationType.None)
        {
            var variation = new ItemVariation(Rand32.Create(), type);
            return new ItemSlotEquip
            {
                TemplateID = template.TemplateID,

                RUC = (byte) template.TUC,
                STR = (short) variation.Get(template.IncSTR),
                DEX = (short) variation.Get(template.IncDEX),
                INT = (short) variation.Get(template.IncINT),
                LUK = (short) variation.Get(template.IncLUK),
                MaxHP = (short) variation.Get(template.IncMaxHP),
                MaxMP = (short) variation.Get(template.IncMaxMP),
                PAD = (short) variation.Get(template.IncPAD),
                MAD = (short) variation.Get(template.IncMAD),
                PDD = (short) variation.Get(template.IncPDD),
                MDD = (short) variation.Get(template.IncMDD),
                ACC = (short) variation.Get(template.IncACC),
                EVA = (short) variation.Get(template.IncEVA),
                Craft = (short) variation.Get(template.IncCraft),
                Speed = (short) variation.Get(template.IncSpeed),
                Jump = (short) variation.Get(template.IncJump),
                Durability = 100
            };
        }

        public static ItemSlotBundle FromTemplate(ItemBundleTemplate template)
        {
            return new ItemSlotBundle
            {
                TemplateID = template.TemplateID,
                MaxNumber = (short) template.MaxPerSlot
            };
        }

        public static ItemSlot FromTemplate(ItemTemplate template,
            ItemVariationType type = ItemVariationType.None)
        {
            switch (template)
            {
                case ItemEquipTemplate equipTemplate:
                    return FromTemplate(equipTemplate, type);
                case ItemBundleTemplate bundleTemplate:
                    return FromTemplate(bundleTemplate);
            }

            return null;
        }
    }
}