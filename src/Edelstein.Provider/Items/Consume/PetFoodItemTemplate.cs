using PKG1;

namespace Edelstein.Provider.Items.Consume
{
    public class PetFoodItemTemplate : ItemBundleTemplate
    {
        public short IncRepleteness { get; set; }

        public override void Parse(int templateId, WZProperty p)
        {
            base.Parse(templateId, p);

            IncRepleteness = p.ResolveFor<short>("spec/inc") ?? 0;
        }
    }
}