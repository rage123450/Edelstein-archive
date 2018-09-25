using PKG1;

namespace Edelstein.Provider.NPC
{
    public class NPCScriptTemplate
    {
        public string Script { get; set; }

        // TODO: convert to proper dates (e.g 20100630 => 2010/06/30)
        public int Start { get; set; }
        public int End { get; set; }

        public static NPCScriptTemplate Parse(WZProperty p)
        {
            return new NPCScriptTemplate
            {
                Script = p.ResolveForOrNull<string>("script"),
                Start = p.ResolveFor<int>("start") ?? 0,
                End = p.ResolveFor<int>("end") ?? 0
            };
        }
    }
}