using System;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class CreateCommand : Command<CreateCommandOption>
    {
        public override string Name => "Create";
        public override string Description => "Creates an specific item";

        public CreateCommand()
        {
            Aliases.Add("Item");
        }

        public override async Task Execute(FieldUser user, CreateCommandOption option)
        {
            var templateID = option.TemplateID;
            
            if (!string.IsNullOrEmpty(option.TemplateName))
            {
                var itemNames = user.Socket.WvsGame.ItemNames;
                var results = itemNames.All
                    .Where(p => p.Value.ToLower().Contains(option.TemplateName.ToLower()))
                    .ToList();

                if (results.Any())
                {
                    templateID = results.Select(r => r.Key).First();
                    if (option.Search)
                        if (!await user.Prompt(speaker =>
                            templateID = speaker.AskMenu(
                                "Which item would you like to create?",
                                results.ToDictionary(r => r.Key, r => $"#z{r.Key}# ({r.Key})")
                            ))
                        )
                            return;
                }
            }

            if (!templateID.HasValue) return;
            var itemTemplates = user.Socket.WvsGame.ItemTemplates;
            var itemTemplate = itemTemplates.Get(templateID.Value);

            await user.ModifyInventory(i =>i.Add(itemTemplate, option.Quantity));
        }
    }

    public class CreateCommandOption : CommandOption
    {
        [Option('s', "search", HelpText = "Searches for the template.")]
        public bool Search { get; set; }

        [Option('n', "name", HelpText = "The item's template name.")]
        public string TemplateName { get; set; }

        [Option('q', "quantity", HelpText = "The item's quantity.")]
        public short Quantity { get; set; } = 1;

        [Value(0, MetaName = "templateID", HelpText = "The item's template ID.")]
        public int? TemplateID { get; set; }
    }
}