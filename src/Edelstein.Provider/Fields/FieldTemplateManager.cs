using PKG1;

namespace Edelstein.Provider.Fields
{
    public class FieldLazyTemplateManager : LazyTemplateManager<FieldTemplate>
    {
        public FieldLazyTemplateManager(PackageCollection collection) : base(collection)
        {
        }

        public override FieldTemplate Load(int templateId)
        {
            return FieldTemplate.Parse(templateId, Collection);
        }
    }
}