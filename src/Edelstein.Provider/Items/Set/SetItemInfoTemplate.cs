using System;
using System.Collections.Generic;
using PKG1;
using System.Linq;

namespace Edelstein.Provider.Items.Set
{
    public class SetItemInfoTemplate
    {
        public int TemplateID { get; set; }
        public int SetCompleteCount { get; set; }

        public ICollection<int> ItemTemplateID { get; set; }
        public IDictionary<int, SetItemEffectTemplate> Effect { get; set; }

        public static SetItemInfoTemplate Parse(int templateId, PackageCollection collection)
        {
            var entry = collection.Resolve($"Etc/SetItemInfo.img/{templateId}");
            return Parse(templateId, entry);
        }

        public static SetItemInfoTemplate Parse(int templateId, WZProperty p)
        {
            return new SetItemInfoTemplate
            {
                TemplateID = templateId,
                SetCompleteCount = p.ResolveFor<int>("completeCount") ?? 0,
                ItemTemplateID = p.Resolve("itemID").Children
                    .Select(c => c.ResolveFor<int>() ?? 0)
                    .ToList(),
                Effect = p.Resolve("Effect").Children
                    .ToDictionary(
                        c => Convert.ToInt32(c.Name),
                        SetItemEffectTemplate.Parse
                    )
            };
        }
    }
}