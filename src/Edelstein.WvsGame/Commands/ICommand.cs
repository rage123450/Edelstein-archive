using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Commands
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        ICollection<string> Aliases { get; }
        ICollection<ICommand> Commands { get; }

        Task Process(FieldUser user, string args);
        Task Process(FieldUser user, Queue<string> args);
    }
}