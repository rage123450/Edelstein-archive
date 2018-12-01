using System;
using System.Drawing;
using PKG1;

namespace Edelstein.Provider.Fields
{
    public class FieldPortalTemplate
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public FieldPortalType Type { get; set; }

        public string Script { get; set; }

        public int ToMap { get; set; }
        public string ToName { get; set; }

        public Point Position { get; set; }

        public static FieldPortalTemplate Parse(WZProperty p)
        {
            var res = new FieldPortalTemplate
            {
                ID = Convert.ToInt32(p.Name),
                Name = p.ResolveForOrNull<string>("pn"),
                Type = (FieldPortalType) (p.ResolveFor<int>("pt") ?? 0),
                Script = p.ResolveForOrNull<string>("script"),
                ToMap = p.ResolveFor<int>("tm") ?? int.MinValue,
                ToName = p.ResolveForOrNull<string>("tn"),
                Position = new Point(
                    p.ResolveFor<int>("x") ?? int.MinValue,
                    p.ResolveFor<int>("y") ?? int.MinValue
                )
            };

            return res;
        }
    }
}