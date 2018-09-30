using System;
using System.Linq;
using MoreLinq;
using PKG1;

namespace Edelstein.Provider.Items
{
    public abstract class ItemTemplate
    {
        public int TemplateID { get; set; }

        public virtual void Parse(int templateId, WZProperty p)
        {
            TemplateID = templateId;
        }

        public static ItemTemplate Parse(int templateId, PackageCollection collection)
        {
            ItemTemplate item;
            var type = templateId / 1000000;

            switch (type)
            {
                case 1:
                {
                    var entry = collection.Resolve("Character").Children
                        .SelectMany(c => c.Children)
                        .FirstOrDefault(c => c.Name == $"{templateId:D8}.img");
                    item = new ItemEquipTemplate();
                    item.Parse(templateId, entry);
                    return item;
                }
                case 2:
                case 3:
                case 4:
                case 5:
                {
                    var entry = collection.Resolve("Item").Children
                        .SelectMany(c => c.Children)
                        .SelectMany(c => c.Children)
                        .FirstOrDefault(c => c.Name == $"{templateId:D8}");
                    item = new ItemBundleTemplate();
                    item.Parse(templateId, entry);
                    return item;
                }
            }

            return null;
        }
    }
}