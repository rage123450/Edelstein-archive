using System.Threading.Tasks;
using CommandLine;
using Edelstein.Provider;
using Edelstein.Provider.Fields;
using Edelstein.WvsGame.Commands.Utils;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class FieldCommand : TemplateCommand<FieldTemplate, FieldCommandOption>
    {
        public override string Name => "Field";
        public override string Description => "Transfers to a specific field.";

        public FieldCommand()
        {
            Aliases.Add("Map");
        }

        public override Task Execute(
            FieldUser user,
            FieldTemplate template,
            FieldCommandOption option
        )
        {
            var field = user.Socket.WvsGame.FieldFactory.Get(template.TemplateID);

            user.Character.FieldPortal = option.PortalID ?? 0;
            field.Enter(user);
            return Task.CompletedTask;
        }

        public override ITemplateManager<FieldTemplate> getTemplates(FieldUser user)
            => user.Socket.WvsGame.FieldTemplates;

        public override EagerTemplateManager<string> getStringTemplates(FieldUser user)
            => user.Socket.WvsGame.FieldNames;
    }

    public class FieldCommandOption : TemplateCommandOption
    {
        [Option('p', "portal", HelpText = "The field's portal template ID.")]
        public byte? PortalID { get; set; }
    }
}