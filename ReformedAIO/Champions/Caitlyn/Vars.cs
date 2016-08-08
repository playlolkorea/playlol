using LeagueSharp;
using LeagueSharp.Common;

namespace ReformedAIO.Champions.Caitlyn
{
    internal class Vars
    {
        public static Orbwalking.Orbwalker Orbwalker { get; internal set; }

        public static Obj_AI_Hero Player => ObjectManager.Player;
    }
}
