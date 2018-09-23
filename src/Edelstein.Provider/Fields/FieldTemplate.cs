using System.Collections.Generic;
using System.Linq;
using PKG1;

namespace Edelstein.Provider.Fields
{
    public class FieldTemplate
    {
        public IDictionary<int, FieldFootholdTemplate> Footholds;
        public ICollection<FieldPortalTemplate> Portals;
        public ICollection<FieldLifeTemplate> Life;

        public static FieldTemplate Parse(int templateId, PackageCollection collection)
        {
            var mapEntry = collection.Resolve($"Map/Map/Map{templateId.ToString("D8")[0]}/{templateId:D8}.img");
            mapEntry = mapEntry ?? collection.Resolve($"Map/Map/Map{templateId.ToString("D9")[0]}/{templateId:D9}.img");

            var link = mapEntry.ResolveFor<int>("info/link");
            return link.HasValue ? Parse(link.Value, collection) : Parse(mapEntry);
        }

        public static FieldTemplate Parse(WZProperty p)
        {
            var res = new FieldTemplate
            {
                Footholds = p.Resolve("foothold").Children
                    .SelectMany(c => c.Children)
                    .SelectMany(c => c.Children)
                    .Select(FieldFootholdTemplate.Parse)
                    .ToDictionary(x => x.ID, x => x),
                Portals = p.Resolve("portal").Children
                    .Select(FieldPortalTemplate.Parse)
                    .ToList(),
                Life = p.Resolve("life").Children
                    .Select(FieldLifeTemplate.Parse)
                    .ToList()
            };

            return res;
        }
    }
}