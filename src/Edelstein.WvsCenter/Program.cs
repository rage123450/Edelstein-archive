using Lamar;

namespace Edelstein.WvsCenter
{
    class Program
    {
        static void Main(string[] args)
        {
            var registry = new WvsCenterRegistry();
            var container = new Container(registry);

            container.GetInstance<WvsCenter>().Run().Wait();
        }
    }
}