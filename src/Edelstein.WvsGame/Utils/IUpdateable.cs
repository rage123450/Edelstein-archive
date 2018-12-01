using System;
using System.Threading.Tasks;

namespace Edelstein.WvsGame.Utils
{
    public interface IUpdateable
    {
        Task Update(DateTime now);
    }
}