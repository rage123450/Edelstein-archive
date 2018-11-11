using PKG1;

namespace Edelstein.Provider.Reactors
{
    public class ReactorLazyTemplateManager : LazyTemplateManager<ReactorTemplate>
    {
        public ReactorLazyTemplateManager(PackageCollection collection) : base(collection)
        {
        }

        public override ReactorTemplate Load(int templateId)
        {
            return ReactorTemplate.Parse(templateId, Collection);
        }
    }
}