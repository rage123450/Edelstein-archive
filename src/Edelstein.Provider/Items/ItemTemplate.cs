using System.Linq;
using Edelstein.Provider.Items.Cash;
using Edelstein.Provider.Items.Consume;
using Edelstein.Provider.Items.Install;
using PKG1;

namespace Edelstein.Provider.Items
{
    public abstract class ItemTemplate
    {
        public int TemplateID { get; set; }

        public int SellPrice { get; set; }
        public bool TimeLimited { get; set; }

        public int ReplaceTemplateID { get; set; }
        public int ReplaceMsg { get; set; }
        public int ReplacePeriod { get; set; }

        public bool Quest { get; set; }
        public bool PartyQuest { get; set; }
        public bool Only { get; set; }
        public bool TradeBlock { get; set; }
        public bool NotSale { get; set; }
        public bool BigSize { get; set; }
        public bool ExpireOnLogout { get; set; }
        public bool AccountSharable { get; set; }

        public bool Cash { get; set; }

        public virtual void Parse(int templateId, WZProperty p)
        {
            TemplateID = templateId;

            SellPrice = p.ResolveFor<int>("info/price") ?? 0;
            TimeLimited = p.ResolveFor<bool>("info/timeLimited") ?? false;

            // TODO: replace

            Quest = p.ResolveFor<bool>("info/quest") ?? false;
            PartyQuest = p.ResolveFor<bool>("info/pquest") ?? false;
            Only = p.ResolveFor<bool>("info/only") ?? false;
            TradeBlock = p.ResolveFor<bool>("info/tradeBlock") ?? false;
            NotSale = p.ResolveFor<bool>("info/notSale") ?? false;
            BigSize = p.ResolveFor<bool>("info/bigSize") ?? false;
            ExpireOnLogout = p.ResolveFor<bool>("info/expireOnLogout") ?? false;
            AccountSharable = p.ResolveFor<bool>("info/accountSharable") ?? false;

            Cash = p.ResolveFor<bool>("info/cash") ?? false;
        }

        public static ItemTemplate Parse(int templateId, PackageCollection collection)
        {
            WZProperty prop = null;
            ItemTemplate item = null;
            var type = templateId / 1000000;
            var subType = templateId % 1000000 / 10000;
            var header = templateId / 10000;

            switch (type)
            {
                case 1:
                    item = new ItemEquipTemplate();
                    prop = collection.Resolve("Character").Children
                        .SelectMany(c => c.Children)
                        .FirstOrDefault(c => c.Name == $"{templateId:D8}.img");
                    break;
                case 2:
                    prop = collection.Resolve($"Item/Consume/{header:D4}.img/{templateId:D8}");
                    switch (subType)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 5:
                        case 21:
                        case 36:
                        case 38:
                        case 45:
                            item = new StatChangeItemTemplate();
                            break;
                        case 3:
                            item = new PortalScrollItemTemplate();
                            break;
                        case 4:
                            item = new UpgradeItemTemplate();
                            break;
                        case 10:
                            item = new MobSummonItemTemplate();
                            break;
                        case 12:
                            item = new PetFoodItemTemplate();
                            break;
                        case 26:
                            item = new TamingMobFoodItemTemplate();
                            break;
                        case 27:
                            item = new BridleItemTemplate();
                            break;
                        case 28:
                        case 29:
                            item = new SkillLearnItemTemplate();
                            break;
                    }

                    break;
                case 3:
                    prop = collection.Resolve($"Item/Install/{header:D4}.img/{templateId:D8}");
                    if (subType == 1) item = new PortableChairItemTemplate();
                    break;
                case 4:
                    prop = collection.Resolve($"Item/Etc/{header:D4}.img/{templateId:D8}");
                    break;
                case 5: // TODO
                    switch (subType)
                    {
                        case 0:
                            item = new PetItemTemplate();
                            prop = collection.Resolve($"Item/Pet/{templateId:D7}.img");
                            break;
                    }

                    prop = prop ?? collection.Resolve($"Item/Cash/{header:D4}.img/{templateId:D8}");
                    break;
            }

            if (item == null) item = new ItemBundleTemplate();

            item.Parse(templateId, prop);
            return item;
        }
    }
}