using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using CSharpx;
using Edelstein.Database.Entities.Inventory;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class EditCommand : Command<EditCommandOption>
    {
        public override string Name => "Edit";
        public override string Description => "Edits an inventory item";

        public override Task Execute(FieldUser user, EditCommandOption option)
        {
            return user.Prompt(speaker =>
            {
                var items = user.Character.GetInventory(option.Type).Items;
                var slot = speaker.AskMenu("Which item would you like to edit?",
                    items
                        .OrderBy(i => i.Position)
                        .ToDictionary(i => (int) i.Position, i => $"#z{i.TemplateID}# ({i.TemplateID})")
                );
                var item = items.FirstOrDefault(i => i.Position == slot);

                if (item == null) return;
                if (option.Destroy)
                {
                    if (speaker.AskYesNo($"Are you sure you would like to destroy #b#z{item.TemplateID}##k?"))
                        user.ModifyInventory(i => i.Remove(item));
                    return;
                }

                switch (item)
                {
                    case ItemSlotEquip equip:
                        equip.Title = speaker.AskText("Specify a title", equip.Title);
                        break;
                    case ItemSlotBundle bundle:
                        bundle.Title = speaker.AskText("Specify a title", bundle.Title);
                        break;
                }

                user.ModifyInventory(i => i.Update(item));
            });
        }
    }

    public class EditCommandOption : CommandOption
    {
        [Option('d', "destroy", HelpText = "Destroys the item slot.")]
        public bool Destroy { get; set; }

        [Value(0, MetaName = "inventoryType", Required = true, HelpText = "The inventory's type.")]
        public ItemInventoryType Type { get; set; }
    }
}