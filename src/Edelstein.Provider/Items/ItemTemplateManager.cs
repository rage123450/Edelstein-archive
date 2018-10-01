using PKG1;

namespace Edelstein.Provider.Items
{
    public class ItemLazyTemplateManager : LazyTemplateManager<ItemTemplate>
    {
        public ItemLazyTemplateManager(PackageCollection collection) : base(collection)
        {
        }

        public override ItemTemplate Load(int templateId)
        {
            return ItemTemplate.Parse(templateId, Collection);
        }
    }
}