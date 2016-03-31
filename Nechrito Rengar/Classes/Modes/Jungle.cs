using System;
using LeagueSharp.Common;
using System.Linq;

namespace Nechrito_Rengar
{
    class Jungle
    {
        private static void Game_OnUpdate(EventArgs args)
        {
            JungleLogic();
        }
        public static void JungleLogic()
        {
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {

                var mobs = MinionManager.GetMinions(190 + Program.Player.AttackRange, MinionTypes.All, MinionTeam.Neutral,
           MinionOrderTypes.MaxHealth);

                if (mobs.Count <= 0 || Program.Player.Mana == 5 && MenuConfig.Passive)
                    return;
                if (Spells._q.IsReady())
                    Spells._q.Cast(mobs[0]);


                if (Spells._e.IsReady() && Program.Player.Mana < 5)
                    Spells._e.Cast(mobs[0]);

                if (Spells._w.IsReady())
                {
                    Spells._w.Cast(mobs[0]);
                    Logic.CastHydra();
                }
                   

            }
        }


    }
}
