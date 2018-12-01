using System.Drawing;
using PKG1;

namespace Edelstein.Provider.Fields
{
    public class FieldReactorTemplate
    {
        public int TemplateID { get; set; }

        public bool F { get; set; }
        public Point Position { get; set; }

        public static FieldReactorTemplate Parse(WZProperty p)
        {
            var res = new FieldReactorTemplate
            {
                TemplateID = p.ResolveFor<int>("id") ?? -1,
                F = p.ResolveFor<bool>("f") ?? false,
                Position = new Point(
                    p.ResolveFor<int>("x") ?? int.MinValue,
                    p.ResolveFor<int>("y") ?? int.MinValue
                )
            };

            return res;
        }
    }
}