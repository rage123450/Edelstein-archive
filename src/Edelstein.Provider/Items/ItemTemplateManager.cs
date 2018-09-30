using PKG1;

namespace Edelstein.Provider.Items
{
    public class ItemTemplateManager : TemplateManager<ItemTemplate>
    {
        public ItemTemplateManager(PackageCollection collection) : base(collection)
        {
        }

        public override ItemTemplate Load(int templateId)
        {
            return ItemTemplate.Parse(templateId, Collection);
        }
    }
}