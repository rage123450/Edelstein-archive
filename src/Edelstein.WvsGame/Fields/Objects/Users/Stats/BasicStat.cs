using System;
using System.Linq;
using Edelstein.Database.Entities.Inventory;
using Edelstein.Provider.Items;
using MoreLinq;

namespace Edelstein.WvsGame.Fields.Objects.Users.Stats
{
    public class BasicStat
    {
        private FieldUser _user;

        public byte Gender { get; set; }
        public byte Level { get; set; }
        public int Job { get; set; }

        public int STR { get; set; }
        public int DEX { get; set; }
        public int INT { get; set; }
        public int LUK { get; set; }

        public int POP { get; set; }

        public int MaxHP { get; set; }
        public int MaxMP { get; set; }

        public BasicStatRateOption Option { get; set; }

        public BasicStat(FieldUser user)
        {
            _user = user;
        }

        public void Calculate()
        {
            var character = _user.Character;
            Gender = character.Gender;
            Level = character.Level;
            Job = character.Job;

            STR = character.STR;
            DEX = character.DEX;
            INT = character.INT;
            LUK = character.LUK;

            POP = character.POP;

            MaxHP = character.MaxHP;
            MaxMP = character.MaxMP;

            var templates = _user.Socket.WvsGame.ItemTemplates;
            var incMaxHPr = 0;
            var incMaxMPr = 0;
            
            character.GetInventory(ItemInventoryType.Equip).Items
                .OfType<ItemSlotEquip>()
                .Where(i => i.Slot < 0)
                .ForEach(i =>
                {
                    STR += i.STR;
                    DEX += i.DEX;
                    INT += i.INT;
                    LUK += i.LUK;

                    MaxHP += i.MaxHP;
                    MaxMP += i.MaxHP;

                    var template = templates.Get(i.TemplateID);
                    if (template is ItemEquipTemplate equipTemplate)
                    { // TODO: and not Dragon or Mechanic
                        // TODO: niMaxHPr, niMaxMPr
                        // TODO: Socket1, Socket2
                        // TODO: Option1, Option2, Option3
                    }
                });
            
            // TODO: Set item

            MaxHP = Math.Min(MaxHP, 99999);
            MaxMP = Math.Min(MaxMP, 99999);
        }
    }
}