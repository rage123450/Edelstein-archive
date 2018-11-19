using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Commands
{
    public interface ICommand<T>
        where T : ICommandOption
    {
        Task Process(FieldUser user, string args);
        Task Process(FieldUser user, Queue<string> args);
        T Parse(IEnumerable<string> args);
        Task Execute(FieldUser user, T option);
    }
}