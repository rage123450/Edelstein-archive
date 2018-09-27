using System.Collections.Generic;
using Edelstein.WvsGame.Fields.Objects;

namespace Edelstein.WvsGame.Commands
{
    public class CommandContext
    {
        public Queue<string> Args { get; }
        public FieldUser User { get; }

        public CommandContext(Queue<string> args, FieldUser user)
        {
            Args = args;
            User = user;
        }
    }
}