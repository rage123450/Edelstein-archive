using PKG1;

namespace Edelstein.Provider.Reactors
{
    public class ReactorTemplate
    {
        public int TemplateID { get; set; }

        public static ReactorTemplate Parse(int templateId, PackageCollection collection)
        {
            var npcEntry = collection.Resolve($"Reactor/{templateId:D7}.img");
            return Parse(templateId, npcEntry);
        }

        public static ReactorTemplate Parse(int templateId, WZProperty p)
        {
            return new ReactorTemplate()
            {
                TemplateID = templateId
            };
        }
    }
}