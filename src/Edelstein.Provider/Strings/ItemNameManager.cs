using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq.Extensions;
using PKG1;

namespace Edelstein.Provider.Strings
{
    public class ItemNameManager : EagerTemplateManager<string>
    {
        public ItemNameManager(PackageCollection collection) : base(collection)
        {
        }

        public override Task LoadAll()
        {
            var prop = Collection.Resolve("String");
            var directories = new List<string> {"Consume", "Ins", "Cash"};

            prop.Resolve("Eqp/Eqp").Children
                .SelectMany(c => c.Children)
                .DistinctBy(c => c.Name)
                .ToDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => c.ResolveForOrNull<string>("name") ?? "NO-NAME"
                )
                .ForEach(Templates.Add);
            prop.Resolve("Etc/Etc").Children
                .DistinctBy(c => c.Name)
                .ToDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => c.ResolveForOrNull<string>("name") ?? "NO-NAME"
                )
                .ForEach(Templates.Add);
            directories
                .Select(d => prop.Resolve($"{d}.img"))
                .SelectMany(c => c.Children)
                .ToDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => c.ResolveForOrNull<string>("name") ?? "NO-NAME"
                )
                .ForEach(Templates.Add);
            return Task.CompletedTask;
        }
    }
}