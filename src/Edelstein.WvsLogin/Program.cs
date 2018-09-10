using System.Threading.Tasks;

namespace Edelstein.WvsLogin
{
    class Program
    {
        static void Main(string[] args) => new WvsLogin().Run().Wait();
    }
}