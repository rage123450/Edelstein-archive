using PKG1;

namespace Edelstein.Provider.NPC
{
    public class NPCTemplateManager : ITemplateManager<NPCTemplate>
    {
        private readonly PackageCollection _collection;

        public NPCTemplateManager(PackageCollection collection)
        {
            _collection = collection;
        }

        public NPCTemplate Get(int templateId)
        {
            return NPCTemplate.Parse(templateId, _collection);
        }
    }
}