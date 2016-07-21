using RethoughtLib.Classes.Intefaces;
using RethoughtLib.Classes.Bootstraps;
using System.Collections.Generic;
using ReformedAIO.Champions;

namespace ReformedAIO
{
    class Program
    {
        static void Main(string[] args)
        {
            RethoughtLib.RethoughtLib.Instance.Load();
            var bootstrap = new Bootstrap(new List<ILoadable>() { new DianaLoader(), new GragasLoader(), new AsheLoader() });
        }
    }
}
