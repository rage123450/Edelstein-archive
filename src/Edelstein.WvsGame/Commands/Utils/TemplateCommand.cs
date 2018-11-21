using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Provider;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Commands.Utils
{
    public abstract class TemplateCommand<T, S> : Command<S>
        where S : TemplateCommandOption
    {
        public override async Task Execute(FieldUser user, S option)
        {
            var templates = getTemplates(user);
            var stringTemplates = getStringTemplates(user);
            var templateID = option.TemplateID;

            if (!string.IsNullOrEmpty(option.Search))
            {
                var results = stringTemplates.All
                    .Where(p => p.Value.ToLower().Contains(option.Search.ToLower()))
                    .ToList();

                if (results.Any())
                {
                    if (!await user.Prompt(speaker =>
                        templateID = speaker.AskMenu(
                            $"Here are the results for '{option.Search}'",
                            results.ToDictionary(r => r.Key, r => $"#z{r.Key}# ({r.Key})")
                        ))
                    ) return;
                }
            }

            if (!templateID.HasValue) return;
            await Execute(user, templates.Get(templateID.Value), option);
        }

        public abstract Task Execute(FieldUser user, T template, S option);

        public abstract ITemplateManager<T> getTemplates(FieldUser user);
        public abstract EagerTemplateManager<string> getStringTemplates(FieldUser user);
    }

    public class TemplateCommandOption : CommandOption
    {
        [Option('s', "search", HelpText = "Searches for the template with string.")]
        public string Search { get; set; }

        [Value(0, MetaName = "templateID", HelpText = "The template ID.")]
        public int? TemplateID { get; set; }
    }
}