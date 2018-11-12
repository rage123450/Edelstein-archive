using System.Collections.Generic;
using System.Linq;
using PKG1;

namespace Edelstein.Provider.Reactors
{
    public class ReactorStateTemplate
    {
        // TODO: timeOut
        public ICollection<ReactorEventTemplate> Events;

        public static ReactorStateTemplate Parse(WZProperty p)
        {
            return new ReactorStateTemplate
            {
                Events = p.Resolve("event")?.Children
                             .Select(ReactorEventTemplate.Parse)
                             .ToList()
                         ?? new List<ReactorEventTemplate>()
            };
        }
    }
}