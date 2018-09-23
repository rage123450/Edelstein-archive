using PKG1;

namespace Edelstein.Provider.Fields
{
    public class FieldTemplateManager : ITemplateManager<FieldTemplate>
    {
        private readonly PackageCollection _collection;

        public FieldTemplateManager(PackageCollection collection)
        {
            this._collection = collection;
        }

        public FieldTemplate Get(int templateId)
        {
            return FieldTemplate.Parse(templateId, _collection);
        }
    }
}