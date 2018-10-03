using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public ICollection<string> Aliases { get; }
        public ICollection<Command> Commands { get; }

        protected Command()
        {
            Aliases = new List<string>();
            Commands = new List<Command>();
        }

        public Command GetCommand(string name)
        {
            return Commands
                .FirstOrDefault(c => c.Name.ToLower().StartsWith(name) ||
                                     c.Aliases.Count(s => s.ToLower().StartsWith(name)) > 0);
        }

        public Task Process(FieldUser user, string text)
        {
            var regex = new Regex("([\"'])(?:(?=(\\\\?))\\2.)*?\\1|([^\\s]+)");
            var args = regex.Matches(text)
                .Select(m =>
                {
                    var res = m.Value;

                    if (res.StartsWith("'") && res.EndsWith("'") ||
                        res.StartsWith("\"") && res.EndsWith("\""))
                    {
                        res = res.Substring(1);
                        res = res.Remove(res.Length - 1);
                    }

                    return res;
                })
                .ToList();

            return Process(new CommandContext(new Queue<string>(args), user));
        }

        protected Task Process(CommandContext ctx)
        {
            var args = ctx.Args;

            if (args.Count > 0)
            {
                var first = args.Peek();
                var command = GetCommand(first);

                if (command != null)
                {
                    args.Dequeue();
                    return command.Process(ctx);
                }
            }

            return Execute(ctx);
        }

        protected abstract Task Execute(CommandContext ctx);
    }
}