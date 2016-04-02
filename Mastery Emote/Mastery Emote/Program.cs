using System;
using LeagueSharp;
using LeagueSharp.Common;


namespace Mastery_Emote
{
    class Program
    {
        public static Menu Menu;
        private static void Main() => CustomEvents.Game.OnGameLoad += OnGameLoad;
        private static void OnGameLoad(EventArgs args)
        {
            Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Mastery Emote</font></b><b><font color=\"#FFFFFF\">]</font></b>");
            Game.OnNotify += OnNotify;
            MenuConfig.LoadMenu();
        }
        private static void OnNotify(GameNotifyEventArgs args)
        {
            if (args.EventId == GameEventId.OnChampionKill && MenuConfig.MasteryEmote)
            {
                Game.Say("/masterybadge");
            }
            if (args.EventId == GameEventId.OnChampionKill && MenuConfig.Laugh)
            {
                Game.Say("/l");
            }
            if (args.EventId == GameEventId.OnChampionKill && MenuConfig.Talk)
            {
                Game.Say("/j");
            }
            if (args.EventId == GameEventId.OnChampionKill && MenuConfig.Taunt)
            {
                Game.Say("/t");
            }
            if (args.EventId == GameEventId.OnChampionKill && MenuConfig.Dance)
            {
                Game.Say("/d");
            }
        }
    }
}
