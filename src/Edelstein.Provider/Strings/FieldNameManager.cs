using System;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq.Extensions;
using PKG1;

namespace Edelstein.Provider.Strings
{
    public class FieldNameManager : EagerTemplateManager<string>
    {
        public FieldNameManager(PackageCollection collection) : base(collection)
        {
        }

        public override Task LoadAll()
        {
            var prop = Collection.Resolve("String/Map.img");

            prop.Children
                .SelectMany(c => c.Children)
                .ToDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => c.ResolveForOrNull<string>("mapName") ?? "NO-NAME"
                )
                .ForEach(Templates.Add);
            return Task.CompletedTask;
        }
    }
}