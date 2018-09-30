using PKG1;

namespace Edelstein.Provider.NPC
{
    public class NPCTemplateManager : TemplateManager<NPCTemplate>
    {
        public NPCTemplateManager(PackageCollection collection) : base(collection)
        {
        }

        public override NPCTemplate Load(int templateId)
        {
            return NPCTemplate.Parse(templateId, Collection);
        }
    }
}