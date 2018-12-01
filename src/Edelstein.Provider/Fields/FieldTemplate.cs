using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoreLinq;
using PKG1;

namespace Edelstein.Provider.Fields
{
    public class FieldTemplate
    {
        public int TemplateID { get; set; }
        
        public Rectangle Bounds { get; set; }
        public Size Size { get; set; }

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

            var footholds = res.Footholds.Values;
            var leftTop = new Point(
                footholds.Select(f => f.X1 > f.X2 ? f.X2 : f.X1).OrderBy(f => f).First(),
                footholds.Select(f => f.Y1 > f.Y2 ? f.Y2 : f.Y1).OrderBy(f => f).First()
            );
            var rightBottom = new Point(
                footholds.Select(f => f.X1 > f.X2 ? f.X1 : f.X2).OrderByDescending(f => f).First(),
                footholds.Select(f => f.Y1 > f.Y2 ? f.Y1 : f.Y2).OrderByDescending(f => f).First()
            );

            leftTop = new Point(
                p.ResolveFor<int>("info/VRLeft") ?? leftTop.X,
                p.ResolveFor<int>("info/VRTop") ?? leftTop.Y
            );
            rightBottom = new Point(
                p.ResolveFor<int>("info/VRRight") ?? rightBottom.X,
                p.ResolveFor<int>("info/VRBottom") ?? rightBottom.Y
            );

            res.Bounds = Rectangle.FromLTRB(leftTop.X, leftTop.Y, rightBottom.X, rightBottom.Y);

            return res;
        }
    }
}