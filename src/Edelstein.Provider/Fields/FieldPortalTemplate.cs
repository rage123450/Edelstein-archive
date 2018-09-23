using PKG1;

namespace Edelstein.Provider.Fields
{
    public class FieldPortalTemplate
    {
        public string Name { get; set; }
        public FieldPortalType Type { get; set; }

        public string Script { get; set; }

        public int ToMap { get; set; }
        public string ToName { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public static FieldPortalTemplate Parse(WZProperty p)
        {
            var res = new FieldPortalTemplate
            {
                Name = p.ResolveForOrNull<string>("pn"),
                Type = (FieldPortalType) (p.ResolveFor<int>("pt") ?? 0),
                Script = p.ResolveForOrNull<string>("script"),
                ToMap = p.ResolveFor<int>("tm") ?? int.MinValue,
                ToName = p.ResolveForOrNull<string>("tn"),
                X = p.ResolveFor<int>("x") ?? int.MinValue,
                Y = p.ResolveFor<int>("y") ?? int.MinValue
            };

            return res;
        }
    }
}