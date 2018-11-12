using System;
using System.Collections.Generic;
using System.Linq;
using PKG1;

namespace Edelstein.Provider.Reactors
{
    public class ReactorStateTemplate
    {
        public int ID { get; set; }
        // TODO: timeOut
        public ICollection<ReactorEventTemplate> Events;

        public static ReactorStateTemplate Parse(WZProperty p)
        {
            return new ReactorStateTemplate
            {
                ID = Convert.ToInt32(p.Name),
                Events = p.Resolve("event")?.Children
                             .Select(ReactorEventTemplate.Parse)
                             .ToList()
                         ?? new List<ReactorEventTemplate>()
            };
        }
    }
}