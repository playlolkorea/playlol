using System;
using LeagueSharp.Common;

namespace Nechrito_Rengar
{
    class Burst
    {
        private static void Game_OnUpdate(EventArgs args)
        {
            BurstLogic();
        }
        public static void BurstLogic()
        {
            var target = TargetSelector.GetSelectedTarget();
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                if (Program.Player.Mana == 5 &&(Program.Player.Distance(target.Position) <= 600f))
                    {
                    if (Spells._q.IsReady())
                        Spells._q.Cast(target);
                    if (Spells._e.IsReady())
                        Spells._e.Cast(target);
                        Logic.CastYoumoo();
                    if (Spells._w.IsReady())
                    {
                        Logic.CastHydra();
                        Spells._w.Cast(target);
                    }
                    if (Program.Player.Mana <= 4)
                    {
                        if (Spells._q.IsReady())
                            Spells._q.Cast(target);

                        if (Spells._w.IsReady())
                        {
                            Logic.CastHydra();
                            Spells._w.Cast(target);
                        }
                        if (Spells._e.IsReady())
                            Spells._e.Cast(target);
                    }
                }
            }
        }
    }
}
