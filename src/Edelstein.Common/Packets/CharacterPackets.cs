using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Inventory;
using Edelstein.Network.Packets;

namespace Edelstein.Common.Packets
{
    public static class CharacterPackets
    {
        public static void EncodeData(this Character c, OutPacket p)
        {
            long flag = -1;
            p.Encode<long>(flag);
            p.Encode<byte>(0);
            p.Encode<byte>(0);

            if ((flag & 0x1) != 0)
            {
                EncodeStats(c, p);
                p.Encode<byte>(250); // nFriendMax
                p.Encode<bool>(false);
            }

            if ((flag & 0x2) != 0) p.Encode<int>(0); // Money
            if ((flag & 0x80) != 0)
            {
                if ((flag & 0x4) != 0) p.Encode<byte>(c.InventoryEquip.SlotMax);
                if ((flag & 0x8) != 0) p.Encode<byte>(c.InventoryConsume.SlotMax);
                if ((flag & 0x10) != 0) p.Encode<byte>(c.InventoryInstall.SlotMax);
                if ((flag & 0x20) != 0) p.Encode<byte>(c.InventoryInstall.SlotMax);
                if ((flag & 0x40) != 0) p.Encode<byte>(c.InventoryCash.SlotMax);
            }

            if ((flag & 0x100000) != 0)
            {
                p.Encode<int>(0);
                p.Encode<int>(0);
            }

            if ((flag & 0x4) != 0)
            {
                void EncodeEquips(IEnumerable<ItemSlot> items)
                {
                    foreach (var i in items)
                    {
                        p.Encode<short>(i.Slot);
                        (i as ItemSlotEquip).Encode(p);
                    }
                }

                EncodeEquips(c.InventoryEquipped.Items);
                p.Encode<short>(0);

                EncodeEquips(c.InventoryEquippedCash.Items);
                p.Encode<short>(0);

                EncodeEquips(c.InventoryEquip.Items);
                p.Encode<short>(0);

                p.Encode<short>(0); // Evan
                p.Encode<short>(0); // Mechanic
            }

            void EncodeBundles(IEnumerable<ItemSlot> items)
            {
                foreach (var i in items)
                {
                    p.Encode<byte>((byte) i.Slot);
                    (i as ItemSlotBundle).Encode(p);
                }
            }

            if ((flag & 0x8) != 0)
            {
                EncodeBundles(c.InventoryConsume.Items);
                p.Encode<byte>(0);
            }

            if ((flag & 0x10) != 0)
            {
                EncodeBundles(c.InventoryInstall.Items);
                p.Encode<byte>(0);
            }

            if ((flag & 0x20) != 0)
            {
                EncodeBundles(c.InventoryEtc.Items);
                p.Encode<byte>(0);
            }

            if ((flag & 0x40) != 0)
            {
                EncodeBundles(c.InventoryCash.Items);
                p.Encode<byte>(0);
            }

            if ((flag & 0x100) != 0) // Skill Record
            {
                p.Encode<short>(0);
            }

            if ((flag & 0x8000) != 0) // Skill Cooltime
            {
                p.Encode<short>(0);
            }

            if ((flag & 0x200) != 0) // Quest Record
            {
                p.Encode<short>(0);
            }

            if ((flag & 0x4000) != 0) // Quest Complete
            {
                p.Encode<short>(0);
            }

            if ((flag & 0x400) != 0) // Minigame Record
            {
                p.Encode<short>(0);
            }

            if ((flag & 0x800) != 0)
            {
                p.Encode<short>(0); // Couple Record
                p.Encode<short>(0); // Friend Record
                p.Encode<short>(0); // Marriage Record
            }

            if ((flag & 0x1000) != 0)
            {
                for (var i = 0; i < 5; i++) p.Encode<int>(0);
                for (var i = 0; i < 10; i++) p.Encode<int>(0);
            }

            if ((flag & 0x40000) != 0)
            {
                p.Encode<short>(0); // New Year Card Record
            }

            if ((flag & 0x80000) != 0)
            {
                p.Encode<short>(0); // Quest Record EX
            }

            if ((flag & 0x200000) != 0)
            {
                if (c.Job / 100 == 33)
                {
                    p.Encode<byte>(0);
                    for (var i = 0; i < 5; i++) p.Encode<int>(0);
                }
            }

            if ((flag & 0x400000) != 0)
            {
                p.Encode<short>(0); // Quest Complete Old
            }

            if ((flag & 0x800000) != 0)
            {
                p.Encode<short>(0); // Visitor Quest Log
            }
        }

        public static void EncodeStats(this Character c, OutPacket p)
        {
            p.Encode<int>(c.ID);
            p.EncodeFixedString(c.Name, 13);

            p.Encode<byte>(c.Gender);
            p.Encode<byte>(c.Skin);
            p.Encode<int>(c.Face);
            p.Encode<int>(c.Hair);

            for (var i = 0; i < 3; i++)
                p.Encode<long>(0); // Pet stuff.

            p.Encode<byte>(c.Level);
            p.Encode<short>(c.Job);
            p.Encode<short>(c.STR);
            p.Encode<short>(c.DEX);
            p.Encode<short>(c.INT);
            p.Encode<short>(c.LUK);
            p.Encode<int>(c.HP);
            p.Encode<int>(c.MaxHP);
            p.Encode<int>(c.MP);
            p.Encode<int>(c.MaxMP);

            p.Encode<short>(c.AP);
            if (c.Job / 1000 != 3 && c.Job / 100 != 22 && c.Job != 2001)
                p.Encode<short>(c.SP);
            else p.Encode<byte>(0); // TODO: extendedSP

            p.Encode<int>(c.EXP);
            p.Encode<short>(c.POP);

            p.Encode<int>(0);
            p.Encode<int>(100000000);
            p.Encode<byte>(0);
            p.Encode<int>(0);
            p.Encode<short>(0);
        }

        public static void EncodeLook(this Character c, OutPacket p)
        {
            p.Encode<byte>(c.Gender);
            p.Encode<byte>(c.Skin);
            p.Encode<int>(c.Face);

            p.Encode<bool>(false);
            p.Encode<int>(c.Hair);

            foreach (var equippedItem in c.InventoryEquipped.Items)
            {
                p.Encode<byte>((byte) equippedItem.Slot);
                p.Encode<int>(equippedItem.TemplateID);
            }

            p.Encode<byte>(0xFF);

            foreach (var equippedCashItem in c.InventoryEquippedCash.Items)
            {
                p.Encode<byte>((byte) equippedCashItem.Slot);
                p.Encode<int>(equippedCashItem.TemplateID);
            }

            p.Encode<byte>(0xFF);

            p.Encode<int>(0); // TODO: nWeaponStickerID

            for (var i = 0; i < 3; i++)
                p.Encode<int>(0);
        }
    }
}