using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using DotNet.Globbing;
using Edelstein.Provider;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Commands.Utils
{
    public abstract class TemplateCommand<T, S> : Command<S>
        where S : TemplateCommandOption
    {
        private readonly GlobOptions _globOptions = new GlobOptions {Evaluation = {CaseInsensitive = true}};

        public override async Task Execute(FieldUser user, S option)
        {
            var templates = getTemplates(user);
            var stringTemplates = getStringTemplates(user);
            var templateID = option.TemplateID;

            if (!string.IsNullOrEmpty(option.Search))
            {
                var glob = Glob.Parse(option.Search, _globOptions);
                var results = stringTemplates.All
                    .Where(p => glob.IsMatch(p.Value))
                    .ToList();
                
                if (!results.Any())
                    results = stringTemplates.All
                        .Where(p => p.Value.ToLower().StartsWith(option.Search.ToLower()))
                        .ToList();

                if (results.Any())
                {
                    if (!await user.Prompt(speaker =>
                        templateID = speaker.AskMenu(
                            $"Here are the results for '{option.Search}'",
                            results.ToDictionary(r => r.Key, r => $"{r.Value} ({r.Key})")
                        ))
                    ) return;
                }
            }

            if (templateID == null) return;
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