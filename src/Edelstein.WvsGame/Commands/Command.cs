using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Commands
{
    public abstract class Command<T> : ICommand<T>
        where T : ICommandOption
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public IEnumerable<string> Aliases { get; }
        public IEnumerable<Command<ICommandOption>> Commands { get; }

        public Command<ICommandOption> GetCommand(string name)
        {
            return Commands.FirstOrDefault(c => c.Name.ToLower().StartsWith(name) ||
                                                c.Aliases.Count(s => s.ToLower().StartsWith(name)) > 0);
        }

        public Task Process(FieldUser user, string input)
        {
            var regex = new Regex("([\"'])(?:(?=(\\\\?))\\2.)*?\\1|([^\\s]+)");
            var args = regex.Matches(input)
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

            return Process(user, new Queue<string>(args));
        }

        public Task Process(FieldUser user, Queue<string> args)
        {
            if (args.Count > 0)
            {
                var first = args.Peek();
                var command = GetCommand(first);

                if (command != null)
                {
                    args.Dequeue();
                    return command.Process(user, args);
                }
            }

            return Execute(user, Parse(args));
        }

        public T Parse(IEnumerable<string> args)
        {
            if (Parser.Default.ParseArguments<T>(args) is Parsed<T> option)
                return option.Value;
            return default(T);
        }

        public abstract Task Execute(FieldUser user, T option);
    }
}