using System.Collections.Generic;
using PKG1;

namespace Edelstein.Provider.Items
{
    public class ItemBundleTemplate : ItemTemplate
    {
        public bool NoCancelMouse { get; set; }

        public ICollection<int> ReqField { get; set; }

        public short MaxPerSlot { get; set; }
        public short Max { get; set; }

        public short ReqQuestOnProgress { get; set; }

        public override void Parse(int templateId, WZProperty p)
        {
            base.Parse(templateId, p);

            MaxPerSlot = p.ResolveFor<short>("info/slotMax") ?? 100;
        }
    }
}