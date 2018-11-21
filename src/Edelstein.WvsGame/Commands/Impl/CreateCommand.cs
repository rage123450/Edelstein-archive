using System.Threading.Tasks;
using CommandLine;
using Edelstein.Provider;
using Edelstein.Provider.Items;
using Edelstein.WvsGame.Commands.Utils;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class CreateCommand : TemplateCommand<ItemTemplate, CreateCommandOption>
    {
        public override string Name => "Create";
        public override string Description => "Creates an specific item";

        public CreateCommand()
        {
            Aliases.Add("Item");
        }

        public override Task Execute(
            FieldUser user,
            ItemTemplate template,
            CreateCommandOption option
        )
            => user.ModifyInventory(i => i.Add(template, option.Quantity));

        public override ITemplateManager<ItemTemplate> getTemplates(FieldUser user)
            => user.Socket.WvsGame.ItemTemplates;

        public override EagerTemplateManager<string> getStringTemplates(FieldUser user)
            => user.Socket.WvsGame.ItemNames;
    }

    public class CreateCommandOption : TemplateCommandOption
    {
        [Option('q', "quantity", HelpText = "The item's quantity.")]
        public short Quantity { get; set; } = 1;
    }
}