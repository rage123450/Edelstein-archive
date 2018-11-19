using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using CSharpx;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Commands
{
    public abstract class Command<T> : ICommand
        where T : CommandOption
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public ICollection<string> Aliases { get; }
        public ICollection<ICommand> Commands { get; }

        private readonly Parser _parser;

        public Command()
        {
            Aliases = new List<string>();
            Commands = new List<ICommand>();
            _parser = new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.CaseInsensitiveEnumValues = true;
            });
        }

        public ICommand GetCommand(string name)
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

            return Parse(user, args);
        }

        public Task Parse(FieldUser user, IEnumerable<string> args)
        {
            return Task.Run(() =>
            {
                var result = _parser.ParseArguments<T>(args);

                result
                    .WithParsed(o => Execute(user, o))
                    .WithNotParsed(errs =>
                        {
                            var helpText = HelpText.AutoBuild(result, _parser.Settings.MaximumDisplayWidth);

                            helpText.Heading = string.Empty;
                            helpText.Copyright = string.Empty;

                            helpText.AddPostOptionsLines(
                                Commands.Select(c => $"{c.Name}\t\t\t\t{c.Description}")
                            );

                            Regex
                                .Split(helpText.ToString(), "\r\n|\r|\n")
                                .ForEach(l => user.Message(l));
                        }
                    );
            });
        }

        public abstract Task Execute(FieldUser user, T option);
    }
}