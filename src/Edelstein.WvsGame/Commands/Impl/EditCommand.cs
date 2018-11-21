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

            switch (item)
            {
                case ItemSlotEquip equip:
                    if (option.BasicStats)
                    {
                        await user.Prompt(s =>
                            equip.STR = (short) s.AskNumber("Specify the STR", equip.STR, max: short.MaxValue));
                        await user.Prompt(s =>
                            equip.DEX = (short) s.AskNumber("Specify the DEX", equip.DEX, max: short.MaxValue));
                        await user.Prompt(s =>
                            equip.INT = (short) s.AskNumber("Specify the INT", equip.INT, max: short.MaxValue));
                        await user.Prompt(s =>
                            equip.LUK = (short) s.AskNumber("Specify the LUK", equip.LUK, max: short.MaxValue));
                        await user.Prompt(s =>
                            equip.MaxHP = (short) s.AskNumber("Specify the Max HP", equip.MaxHP, max: short.MaxValue));
                        await user.Prompt(s =>
                            equip.MaxMP = (short) s.AskNumber("Specify the Max MP", equip.MaxMP, max: short.MaxValue));
                    }

                    if (option.SecondaryStats)
                    {
                        await user.Prompt(s =>
                            equip.PAD = (short) s.AskNumber("Specify the PAD", equip.PAD, max: short.MaxValue));
                        await user.Prompt(s =>
                            equip.MAD = (short) s.AskNumber("Specify the MAD", equip.MAD, max: short.MaxValue));
                        await user.Prompt(s =>
                            equip.PDD = (short) s.AskNumber("Specify the PDD", equip.PDD, max: short.MaxValue));
                        await user.Prompt(s =>
                            equip.MDD = (short) s.AskNumber("Specify the MDD", equip.MDD, max: short.MaxValue));
                        await user.Prompt(s =>
                            equip.ACC = (short) s.AskNumber("Specify the ACC", equip.ACC, max: short.MaxValue));
                        await user.Prompt(s =>
                            equip.EVA = (short) s.AskNumber("Specify the EVA", equip.EVA, max: short.MaxValue));
                        await user.Prompt(s =>
                            equip.Craft = (short) s.AskNumber("Specify the Craft", equip.Craft, max: short.MaxValue));
                        await user.Prompt(s =>
                            equip.Speed = (short) s.AskNumber("Specify the Speed", equip.Speed, max: short.MaxValue));
                        await user.Prompt(s =>
                            equip.Jump = (short) s.AskNumber("Specify the Jump", equip.Jump, max: short.MaxValue));
                    }

                    await user.Prompt(s => equip.Title = s.AskText("Specify the Title", equip.Title));
                    break;
                case ItemSlotBundle bundle:
                    await user.Prompt(s =>
                        bundle.Number = (short) s.AskNumber("Specify the Number", bundle.Number, max: short.MaxValue));
                    await user.Prompt(s =>
                        bundle.MaxNumber = (short) s.AskNumber("Specify the Max Number", bundle.MaxNumber,
                            max: short.MaxValue));
                    await user.Prompt(s => bundle.Title = s.AskText("Specify the Title", bundle.Title));
                    break;
            }

            await target.ModifyInventory(i => i.Update(item));
        }
    }

    public class EditCommandOption : CommandOption
    {
        [Option('t', "target", HelpText = "The target in the current field.")]
        public string Target { get; set; }

        [Option('d', "destroy", HelpText = "Destroys the item slot.")]
        public bool Destroy { get; set; }

        [Option('b', "basicstats", HelpText = "Allows editing of basic equip stats.")]
        public bool BasicStats { get; set; } = true;

        [Option('s', "secondarystats", HelpText = "Allows editing of secondary equip stats.")]
        public bool SecondaryStats { get; set; } = true;

        [Value(0, MetaName = "inventoryType", Required = true, HelpText = "The inventory's type.")]
        public ItemInventoryType Type { get; set; }
    }
}