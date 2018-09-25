using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq.Extensions;
using PKG1;

namespace Edelstein.Provider.NPC
{
    public class NPCTemplate
    {
        public int TemplateID { get; set; }

        public int TrunkPut { get; set; }
        public int TrunkGet { set; get; }

        public bool Trunk => TrunkPut > 0 || TrunkGet > 0;
        public bool StoreBank { get; set; }
        public bool Parcel { get; set; }

        public ICollection<NPCScriptTemplate> Scripts;

        public static NPCTemplate Parse(int templateId, PackageCollection collection)
        {
            var npcEntry = collection.Resolve($"Npc/{templateId:D7}.img");
            return Parse(templateId, npcEntry);
        }

        public static NPCTemplate Parse(int templateId, WZProperty p)
        {
            return new NPCTemplate
            {
                TemplateID = templateId,
                TrunkPut = p.ResolveFor<int>("info/trunkPut") ?? 0,
                TrunkGet = p.ResolveFor<int>("info/trunkGet") ?? 0,
                StoreBank = p.ResolveFor<bool>("info/storeBank") ?? false,
                Parcel = p.ResolveFor<bool>("info/parcel") ?? false,
                Scripts = p.Resolve("info/script")?.Children
                              .Select(NPCScriptTemplate.Parse)
                              .ToList()
                          ?? new List<NPCScriptTemplate>()
            };
        }
    }
}