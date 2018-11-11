using PKG1;

namespace Edelstein.Provider.Items
{
    public class ItemTemplateManager : LazyTemplateManager<ItemTemplate>
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