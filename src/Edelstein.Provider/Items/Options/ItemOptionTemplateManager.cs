using System;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq.Extensions;
using PKG1;

namespace Edelstein.Provider.Items.Options
{
    public class ItemOptionTemplateManager : EagerTemplateManager<ItemOptionTemplate>
    {
        public ItemOptionTemplateManager(PackageCollection collection) : base(collection)
        {
        }

        public override Task LoadAll()
        {
            var prop = Collection.Resolve("Item/ItemOption.img");
            
            prop.Children
                .ToDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => ItemOptionTemplate.Parse(Convert.ToInt32(c.Name), c)
                )
                .ForEach(Templates.Add);
            return Task.CompletedTask;
        }
    }
}