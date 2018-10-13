using Edelstein.Database.Entities.Shop;
using Edelstein.Network.Packets;
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

        public OutPacket GetCreatePacket()
        {
            using (var p = new OutPacket(GameSendOperations.OpenShopDlg))
            {
                p.Encode<int>(_shop.TemplateID);

                p.Encode<short>((short) _shop.Items.Count);
                _shop.Items.ForEach(i =>
                {
                    p.Encode<int>(i.TemplateID);
                    p.Encode<int>(i.Price);
                    p.Encode<byte>(i.DiscountRate);
                    p.Encode<int>(i.TokenTemplateID);
                    p.Encode<int>(i.TokenPrice);
                    p.Encode<int>(i.ItemPeriod);
                    p.Encode<int>(i.LevelLimited);

                    var type = i.TemplateID / 10000;
                    if (type != 207 && type != 233) p.Encode<short>(i.Quantity);
                    else p.Encode<double>(i.UnitPrice);

                    p.Encode<short>(i.MaxPerSlot);
                });
                return p;
            }
        }
    }
}