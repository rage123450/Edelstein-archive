using PKG1;

namespace Edelstein.Provider.Fields
{
    public class FieldReactorTemplate
    {
        public int TemplateID { get; set; }

        public bool F { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public static FieldReactorTemplate Parse(WZProperty p)
        {
            var res = new FieldReactorTemplate
            {
                TemplateID = p.ResolveFor<int>("id") ?? -1,
                F = p.ResolveFor<bool>("f") ?? false ,
                X = p.ResolveFor<int>("x") ?? int.MinValue,
                Y = p.ResolveFor<int>("y") ?? int.MinValue
            };

            return res;
        }
    }
}