using PKG1;

namespace Edelstein.Provider.Mobs
{
    public class MobTemplateManager : ITemplateManager<MobTemplate>
    {
        private readonly PackageCollection _collection;

        public MobTemplateManager(PackageCollection collection)
        {
            _collection = collection;
        }

        public MobTemplate Get(int templateId)
        {
            return MobTemplate.Parse(templateId, _collection);
        }
    }
}