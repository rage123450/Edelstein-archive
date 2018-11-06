using System.Collections.Generic;
using Edelstein.Common.Packets.Stats;
using Edelstein.Database.Entities.Inventory;
using Edelstein.Provider.Items;
using Edelstein.Provider.Items.Cash;
using Edelstein.Provider.Items.Consume;

namespace Edelstein.Common.Utils.Items
{
    public static class ItemInfo
    {
        private static ItemSlotEquip ToItemSlot(this ItemEquipTemplate template,
            ItemVariationType type = ItemVariationType.None)
        {
            var variation = new ItemVariation(Rand32.Create(), type);
            return new ItemSlotEquip
            {
                TemplateID = template.TemplateID,

                RUC = template.TUC,
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

        private static ItemSlotBundle ToItemSlot(this ItemBundleTemplate template)
        {
            return new ItemSlotBundle
            {
                TemplateID = template.TemplateID,
                MaxNumber = template.MaxPerSlot
            };
        }

        private static ItemSlotPet ToItemSlot(this PetItemTemplate template)
        {
            return new ItemSlotPet
            {
                TemplateID = template.TemplateID
            };
        }

        public static ItemSlot ToItemSlot(this ItemTemplate template,
            ItemVariationType type = ItemVariationType.None)
        {
            switch (template)
            {
                case ItemEquipTemplate equipTemplate:
                    return equipTemplate.ToItemSlot(type);
                case ItemBundleTemplate bundleTemplate:
                    return bundleTemplate.ToItemSlot();
                case PetItemTemplate petTemplate:
                    return petTemplate.ToItemSlot();
            }

            return null;
        }

        public static IDictionary<TemporaryStatType, short> GetTemporaryStats(this StatChangeItemTemplate template)
        {
            var temporaryStats = new Dictionary<TemporaryStatType, short>();

            if (template.PAD > 0) temporaryStats.Add(TemporaryStatType.PAD, template.PAD);
            if (template.PDD > 0) temporaryStats.Add(TemporaryStatType.PDD, template.PDD);
            if (template.MAD > 0) temporaryStats.Add(TemporaryStatType.MAD, template.MAD);
            if (template.MDD > 0) temporaryStats.Add(TemporaryStatType.MDD, template.MDD);
            if (template.ACC > 0) temporaryStats.Add(TemporaryStatType.ACC, template.ACC);
            if (template.EVA > 0) temporaryStats.Add(TemporaryStatType.EVA, template.EVA);
            if (template.Craft > 0) temporaryStats.Add(TemporaryStatType.Craft, template.Craft);
            if (template.Speed > 0) temporaryStats.Add(TemporaryStatType.Speed, template.Speed);
            if (template.Jump > 0) temporaryStats.Add(TemporaryStatType.Jump, template.Jump);
            if (template.Morph > 0) temporaryStats.Add(TemporaryStatType.Morph, template.Morph);

            return temporaryStats;
        }
    }
}