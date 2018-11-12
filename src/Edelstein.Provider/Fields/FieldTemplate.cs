using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using PKG1;

namespace Edelstein.Provider.Fields
{
    public class FieldTemplate
    {
        public int TemplateID { get; set; }
        public IDictionary<int, FieldFootholdTemplate> Footholds;
        public IDictionary<int, FieldPortalTemplate> Portals;
        public ICollection<FieldLifeTemplate> Life;
        public ICollection<FieldReactorTemplate> Reactors;

        public static FieldTemplate Parse(int templateId, PackageCollection collection)
        {
            var mapEntry = collection.Resolve($"Map/Map/Map{templateId.ToString("D8")[0]}/{templateId:D8}.img");
            mapEntry = mapEntry ?? collection.Resolve($"Map/Map/Map{templateId.ToString("D9")[0]}/{templateId:D9}.img");

            var link = mapEntry.ResolveFor<int>("info/link");
            return link.HasValue ? Parse(link.Value, collection) : Parse(templateId, mapEntry);
        }

        public static FieldTemplate Parse(int templateId, WZProperty p)
        {
            var res = new FieldTemplate
            {
                TemplateID = templateId,
                Footholds = p.Resolve("foothold").Children
                    .SelectMany(c => c.Children)
                    .SelectMany(c => c.Children)
                    .Select(FieldFootholdTemplate.Parse)
                    .DistinctBy(x => x.ID) // 211040101 has duplicate footholds
                    .ToDictionary(x => x.ID, x => x),
                Portals = p.Resolve("portal").Children
                    .Select(FieldPortalTemplate.Parse)
                    .DistinctBy(x => x.ID)
                    .ToDictionary(x => x.ID, x => x),
                Life = p.Resolve("life").Children
                    .Select(FieldLifeTemplate.Parse)
                    .ToList(),
                Reactors = p.Resolve("reactor").Children
                    .Select(FieldReactorTemplate.Parse)
                    .ToList()
            };

            return res;
        }
    }
}