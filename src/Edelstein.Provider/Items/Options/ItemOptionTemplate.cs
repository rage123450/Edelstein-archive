using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Provider.Mobs;
using PKG1;

namespace Edelstein.Provider.Items.Options
{
    public class ItemOptionTemplate
    {
        public int TemplateID { get; set; }

        public short ReqLevel { get; set; }
        public int OptionType { get; set; }
        public IDictionary<int, ItemOptionLevelTemplate> LevelData;

        public static ItemOptionTemplate Parse(int templateId, PackageCollection collection)
        {
            var entry = collection.Resolve($"Item/ItemOption.img/{templateId:D6}");
            return Parse(templateId, entry);
        }

        public static ItemOptionTemplate Parse(int templateId, WZProperty p)
        {
            return new ItemOptionTemplate
            {
                TemplateID = templateId,
                ReqLevel = p.ResolveFor<short>("info/reqLevel") ?? 0,
                OptionType = p.ResolveFor<short>("info/optionType") ?? 0,
                LevelData = p.Resolve("level").Children
                    .ToDictionary(
                        c => Convert.ToInt32(c.Name),
                        ItemOptionLevelTemplate.Parse
                    )
            };
        }
    }
}