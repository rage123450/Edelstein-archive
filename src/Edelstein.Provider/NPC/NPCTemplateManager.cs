using PKG1;

namespace Edelstein.Provider.NPC
{
    public class NpcLazyTemplateManager : LazyTemplateManager<NPCTemplate>
    {
        public NpcLazyTemplateManager(PackageCollection collection) : base(collection)
        {
        }

        public override NPCTemplate Load(int templateId)
        {
            return NPCTemplate.Parse(templateId, Collection);
        }
    }
}