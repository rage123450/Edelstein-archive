using System;
using PKG1;

namespace Edelstein.Provider.Fields
{
    public class FieldFootholdTemplate
    {
        public int ID { get; set; }
        public int Next { get; set; }
        public int Prev { get; set; }
        public int X1 { get; set; }
        public int X2 { get; set; }
        public int Y1 { get; set; }
        public int Y2 { get; set; }

        public static FieldFootholdTemplate Parse(WZProperty p)
        {
            var res = new FieldFootholdTemplate
            {
                ID = Convert.ToInt32(p.Name),
                Next = p.ResolveFor<int>("next") ?? 0,
                Prev = p.ResolveFor<int>("prev") ?? 0,
                X1 = p.ResolveFor<int>("x1") ?? 0,
                X2 = p.ResolveFor<int>("x2") ?? 0,
                Y1 = p.ResolveFor<int>("y1") ?? 0,
                Y2 = p.ResolveFor<int>("y2") ?? 0
            };

            return res;
        }
    }
}