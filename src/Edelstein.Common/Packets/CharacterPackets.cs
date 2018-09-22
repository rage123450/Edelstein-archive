using Edelstein.Database.Entities;
using Edelstein.Network.Packets;

namespace Edelstein.Common.Packets
{
    public static class CharacterPackets
    {
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
            p.Encode<int>(0);
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