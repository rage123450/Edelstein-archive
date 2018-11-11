using PKG1;

namespace Edelstein.Provider.Reactors
{
    public class ReactorTemplateManager : LazyTemplateManager<ReactorTemplate>
    {
        public ReactorTemplateManager(PackageCollection collection) : base(collection)
        {
        }

        public override ReactorTemplate Load(int templateId)
        {
            return ReactorTemplate.Parse(templateId, Collection);
        }
    }
}