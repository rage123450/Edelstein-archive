using PKG1;

namespace Edelstein.Provider.NPC
{
    public class NpcTemplateManager : LazyTemplateManager<NPCTemplate>
    {
        public NpcTemplateManager(PackageCollection collection) : base(collection)
        {
        }

        public override NPCTemplate Load(int templateId)
        {
            return NPCTemplate.Parse(templateId, Collection);
        }
    }
}