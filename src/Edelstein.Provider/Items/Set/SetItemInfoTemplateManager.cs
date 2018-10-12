using System;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq.Extensions;
using PKG1;

namespace Edelstein.Provider.Items.Set
{
    public class SetItemInfoTemplateManager : EagerTemplateManager<SetItemInfoTemplate>
    {
        public SetItemInfoTemplateManager(PackageCollection collection) : base(collection)
        {
        }

        public override Task LoadAll()
        {
            var prop = Collection.Resolve("Etc/SetItemInfo.img");

            prop.Children
                .ToDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => SetItemInfoTemplate.Parse(Convert.ToInt32(c.Name), c)
                )
                .ForEach(Templates.Add);
            return Task.CompletedTask;
        }
    }
}