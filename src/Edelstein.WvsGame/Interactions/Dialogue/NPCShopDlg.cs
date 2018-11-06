using System;
using System.Linq;
using Edelstein.Common.Utils.Extensions;
using Edelstein.Common.Utils.Items;
using Edelstein.Database.Entities.Inventory;
using Edelstein.Database.Entities.Shop;
using Edelstein.Network.Packets;
using Edelstein.WvsGame.Fields.Objects.Users;
using Edelstein.WvsGame.Packets;
using MoreLinq;

namespace Edelstein.WvsGame.Interactions.Dialogue
{
    public class NPCShopDlg : Dialogue
    {
        private readonly NPCShop _shop;

        public NPCShopDlg(NPCShop shop)
        {
            _shop = shop;
        }

        public override bool OnPacket(FieldUser user, GameRecvOperations operation, InPacket packet)
        {
            if (operation == GameRecvOperations.UserShopRequest)
                OnUserShopRequest(user, packet);

            return true;
        }

        private void OnUserShopRequest(FieldUser user, InPacket packet)
        {
            var type = packet.Decode<byte>();

            switch (type)
            {
                case 0: // Buy
                {
                    var pos = packet.Decode<short>();
                    var templateID = packet.Decode<int>();
                    var count = packet.Decode<short>();
                    var shopItem = _shop.Items
                        .OrderBy(i => i.Position)
                        .ToList()[pos];

                    using (var p = new OutPacket(GameSendOperations.ShopResult))
                    {
                        byte result = 0x0;

                        if (shopItem != null)
                        {
                            if (shopItem.TemplateID != templateID) result = 0x10;
                            if (shopItem.Quantity > 1) count = 1;
                            if (count > shopItem.MaxPerSlot) count = shopItem.MaxPerSlot;
                            if (shopItem.Price > 0)
                                if (user.Character.Money < shopItem.Price * count)
                                    result = 0xA;
                            if (shopItem.TokenTemplateID > 0)
                                if (user.Character.GetItemCount(shopItem.TokenTemplateID) <
                                    shopItem.TokenPrice * count)
                                    result = 0xD;
                            if (shopItem.Stock == 0) result = 0x1;
                            // TODO: level limits

                            var templates = user.Socket.WvsGame.ItemTemplates;
                            var item = templates.Get(shopItem.TemplateID).ToItemSlot();

                            if (item is ItemSlotBundle bundle) bundle.Number = (short) (count * shopItem.Quantity);
                            if (!user.Character.HasSlotFor(item)) result = 0x3;

                            if (result == 0x0)
                            {
                                if (shopItem.Price > 0)
                                    user.ModifyStats(s => s.Money -= shopItem.Price * count);
                                if (shopItem.TokenTemplateID > 0)
                                    user.ModifyInventory(i => i.Remove(
                                        shopItem.TokenTemplateID,
                                        shopItem.TokenPrice * count
                                    ));
                                if (shopItem.Stock > 0) shopItem.Stock--;

                                user.ModifyInventory(i => i.Add(item));
                            }
                        }
                        else result = 0x10;

                        p.Encode<byte>(result);
                        user.SendPacket(p);
                    }

                    break;
                }
                case 1: // Sell
                {
                    var pos = packet.Decode<short>();
                    var templateID = packet.Decode<int>();
                    var count = packet.Decode<short>();
                    var inventory = user.Character.GetInventory((ItemInventoryType) (templateID / 1000000));
                    var item = inventory.Items.FirstOrDefault(i => i.Position == pos);

                    using (var p = new OutPacket(GameSendOperations.ShopResult))
                    {
                        byte result = 0x0;

                        if (item != null)
                        {
                            user.ModifyInventory(i =>
                            {
                                if (item is ItemSlotBundle bundle)
                                {
                                    if (!ItemInfo.IsRechargeableItem(item.TemplateID))
                                    {
                                        if (count < bundle.Number)
                                        {
                                            bundle.Number -= count;
                                            i.UpdateQuantity(bundle);
                                            return;
                                        }
                                    }
                                }

                                count = 1;
                                i.Remove(item);
                            });

                            var templates = user.Socket.WvsGame.ItemTemplates;
                            var template = templates.Get(item.TemplateID);
                            var price = template.SellPrice * count;

                            if (ItemInfo.IsRechargeableItem(item.TemplateID))
                                price += ((ItemSlotBundle) item).Number;

                            user.ModifyStats(s => s.Money += price);
                        }
                        else result = 0x10;

                        p.Encode<byte>(result);
                        user.SendPacket(p);
                    }

                    break;
                }
                case 2: // Recharge
                {
                    // TODO: recharge
                    using (var p = new OutPacket(GameSendOperations.ShopResult))
                    {
                        p.Encode<byte>(0x3);
                        user.SendPacket(p);
                    }

                    break;
                }
                case 3: // Close
                    user.Dialogue = null;
                    break;
            }
        }

        public override OutPacket GetCreatePacket()
        {
            using (var p = new OutPacket(GameSendOperations.OpenShopDlg))
            {
                p.Encode<int>(_shop.TemplateID);

                var items = _shop.Items.ToList();

                p.Encode<short>((short) items.Count);
                items
                    .OrderBy(i => i.Position)
                    .ForEach(i =>
                    {
                        p.Encode<int>(i.TemplateID);
                        p.Encode<int>(i.Price);
                        p.Encode<byte>(i.DiscountRate);
                        p.Encode<int>(i.TokenTemplateID);
                        p.Encode<int>(i.TokenPrice);
                        p.Encode<int>(i.ItemPeriod);
                        p.Encode<int>(i.LevelLimited);

                        if (!ItemInfo.IsRechargeableItem(i.TemplateID)) p.Encode<short>(i.Quantity);
                        else p.Encode<double>(i.UnitPrice);

                        p.Encode<short>(i.MaxPerSlot);
                    });
                return p;
            }
        }
    }
}