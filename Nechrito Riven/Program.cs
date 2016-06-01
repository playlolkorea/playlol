#region

using System;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace NechritoRiven
{
    public class Program
    {
        private static void Main()
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Riven")
            {
                Game.PrintChat("Could not load Riven");
                return;
            }
           Load.Load.LoadAssembly();
        }
    }
}