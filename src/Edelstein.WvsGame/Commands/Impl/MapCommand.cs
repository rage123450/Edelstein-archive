using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class MapCommand : Command<MapCommandOption>
    {
        public override string Name => "Field";
        public override string Description => "Transfers to a specific field.";

        public MapCommand()
        {
            Aliases.Add("Map");
        }

        public override async Task Execute(FieldUser user, MapCommandOption option)
        {
            var templateID = option.TemplateID ?? user.Field.ID;

            if (!string.IsNullOrEmpty(option.TemplateName))
            {
                var fieldNames = user.Socket.WvsGame.FieldNames;
                var results = fieldNames.All
                    .Where(p => p.Value.ToLower().Contains(option.TemplateName.ToLower()))
                    .ToList();

                if (results.Any())
                {
                    templateID = results.Select(r => r.Key).First();
                    if (option.Search)
                        if (!await user.Prompt(speaker =>
                            templateID = speaker.AskMenu(
                                "Which field would you like to transfer to?",
                                results.ToDictionary(r => r.Key, r => $"{r.Value} ({r.Key})")
                            ))
                        )
                            return;
                }
            }

            var fieldFactory = user.Socket.WvsGame.FieldFactory;
            var field = fieldFactory.Get(templateID);

            user.Character.FieldPortal = option.PortalID ?? 0;
            field.Enter(user);

            if (option.Verbose)
            {
                var fieldNames = user.Socket.WvsGame.FieldNames;
                await user.Message($"Transferring to field {fieldNames.Get(templateID)} ({templateID})");
            }
        }
    }

    public class MapCommandOption : CommandOption
    {
        [Option('s', "search", HelpText = "Searches for the template.")]
        public bool Search { get; set; }

        [Option('n', "name", HelpText = "The field's template name.")]
        public string TemplateName { get; set; }

        [Option('p', "portal", HelpText = "The field's portal template ID.")]
        public byte? PortalID { get; set; }

        [Value(0, MetaName = "templateID", HelpText = "The field's template ID.")]
        public int? TemplateID { get; set; }
    }
}