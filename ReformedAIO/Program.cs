using RethoughtLib.Classes.Intefaces;
using System.Collections.Generic;
using ReformedAIO.Champions;

namespace ReformedAIO
{
    internal class Program
    {
        private static void Main()
        {
            RethoughtLib.RethoughtLib.Instance.Load();

            var bootstrap = new Bootstrap(new List<ILoadable>
            {
                new DianaLoader(), new GragasLoader(), new AsheLoader(), new RyzeLoader()
            });
        }
    }
}
