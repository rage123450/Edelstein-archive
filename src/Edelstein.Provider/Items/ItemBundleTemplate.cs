using PKG1;

namespace Edelstein.Provider.Items
{
    public class ItemBundleTemplate : ItemTemplate
    {
        public int MaxPerSlot { get; set; }

        public override void Parse(int templateId, WZProperty p)
        {
            base.Parse(templateId, p);

            MaxPerSlot = p.ResolveFor<int>("info/slotMax") ?? 100;
        }
    }
}