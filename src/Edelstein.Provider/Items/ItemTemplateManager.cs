using PKG1;

namespace Edelstein.Provider.Items
{
    public class ItemTemplateManager : ITemplateManager<ItemTemplate>
    {
        private readonly PackageCollection _collection;

        public ItemTemplateManager(PackageCollection collection)
        {
            _collection = collection;
        }

        public ItemTemplate Get(int templateId)
        {
            return ItemTemplate.Parse(templateId, _collection);
        }
    }
}