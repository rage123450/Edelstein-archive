using System.Collections.Generic;
using System.Linq;
using PKG1;

namespace Edelstein.Provider.Reactors
{
    public class ReactorTemplate
    {
        public int TemplateID { get; set; }
        public int StateCount => States.Count;
        public bool Move { get; set; }
        public bool NotHitable { get; set; }
        public bool ActivateByTouch { get; set; }
        public int QuestID { get; set; }

        public IDictionary<int, ReactorStateTemplate> States;

        public static ReactorTemplate Parse(int templateId, PackageCollection collection)
        {
            var reactorEntry = collection.Resolve($"Reactor/{templateId:D7}.img");

            var link = reactorEntry.ResolveFor<int>("info/link");
            return link.HasValue ? Parse(link.Value, collection) : Parse(templateId, reactorEntry);
        }

        public static ReactorTemplate Parse(int templateId, WZProperty p)
        {
            return new ReactorTemplate
            {
                TemplateID = templateId,
                States = p.Children
                    .Where(c => c.Name.All(char.IsDigit))
                    .Select(ReactorStateTemplate.Parse)
                    .ToDictionary(c => c.ID, c => c)
            };
        }
    }
}