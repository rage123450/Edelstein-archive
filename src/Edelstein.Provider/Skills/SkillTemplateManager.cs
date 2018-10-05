using PKG1;

namespace Edelstein.Provider.Skills
{
    public class SkillTemplateManager : LazyTemplateManager<SkillTemplate>
    {
        public SkillTemplateManager(PackageCollection collection) : base(collection)
        {
        }

        public override SkillTemplate Load(int templateId)
        {
            return SkillTemplate.Parse(templateId, Collection);
        }
    }
}