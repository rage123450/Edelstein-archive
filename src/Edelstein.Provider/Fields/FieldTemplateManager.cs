using PKG1;

namespace Edelstein.Provider.Fields
{
    public class FieldTemplateManager : LazyTemplateManager<FieldTemplate>
    {
        public FieldTemplateManager(PackageCollection collection) : base(collection)
        {
        }

        public override FieldTemplate Load(int templateId)
        {
            return FieldTemplate.Parse(templateId, Collection);
        }
    }
}