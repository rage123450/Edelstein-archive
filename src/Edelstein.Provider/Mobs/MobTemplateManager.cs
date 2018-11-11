using PKG1;

namespace Edelstein.Provider.Mobs
{
    public class MobTemplateManager : LazyTemplateManager<MobTemplate>
    {
        public MobTemplateManager(PackageCollection collection) : base(collection)
        {
        }

        public override MobTemplate Load(int templateId)
        {
            return MobTemplate.Parse(templateId, Collection);
        }
    }
}