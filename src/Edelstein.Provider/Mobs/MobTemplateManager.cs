using PKG1;

namespace Edelstein.Provider.Mobs
{
    public class MobLazyTemplateManager : LazyTemplateManager<MobTemplate>
    {
        public MobLazyTemplateManager(PackageCollection collection) : base(collection)
        {
        }

        public override MobTemplate Load(int templateId)
        {
            return MobTemplate.Parse(templateId, Collection);
        }
    }
}