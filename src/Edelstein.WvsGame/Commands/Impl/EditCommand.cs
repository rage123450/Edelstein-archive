using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Database.Entities.Inventory;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class EditCommand : Command<EditCommandOption>
    {
        public override string Name => "Edit";
        public override string Description => "Edits an inventory item";

        public override async Task Execute(FieldUser user, EditCommandOption option)
        {
            var target = user.Field.Objects
                             .OfType<FieldUser>()
                             .FirstOrDefault(u => u.Character.Name.ToLower().Equals(option.Target))
                         ?? user;
            ItemSlot item = null;

            await user.Prompt(speaker =>
            {
                var items = target.Character.GetInventory(option.Type).Items;
                var slot = speaker.AskMenu(
                    $"Which item would you like to edit in #r{target.Character.Name}#k's {option.Type} inventory?",
                    items
                        .OrderBy(i => i.Position)
                        .ToDictionary(i => (int) i.Position, i => $"#z{i.TemplateID}# ({i.TemplateID})")
                );
                item = items.FirstOrDefault(i => i.Position == slot);
            });

            if (item == null) return;
            if (option.Destroy)
            {
                await user.Prompt(speaker =>
                {
                    if (!option.Destroy) return;
                    if (speaker.AskYesNo($"Are you sure you would like to destroy #b#z{item.TemplateID}##k?"))
                        target.ModifyInventory(i => i.Remove(item));
                });
                return;
            }

            await user.Prompt(speaker =>
            {
                switch (item)
                {
                    case ItemSlotEquip equip:
                        equip.Title = speaker.AskText("Specify a title", equip.Title);
                        break;
                    case ItemSlotBundle bundle:
                        bundle.Title = speaker.AskText("Specify a title", bundle.Title);
                        break;
                }
            });
            await target.ModifyInventory(i => i.Update(item));
        }
    }

    public class EditCommandOption : CommandOption
    {
        [Option('t', "target", HelpText = "The target in the current field.")]
        public string Target { get; set; }

        [Option('d', "destroy", HelpText = "Destroys the item slot.")]
        public bool Destroy { get; set; }

        [Value(0, MetaName = "inventoryType", Required = true, HelpText = "The inventory's type.")]
        public ItemInventoryType Type { get; set; }
    }
}