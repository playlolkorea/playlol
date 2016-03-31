using System;
using LeagueSharp.Common;
using SPrediction;

namespace Nechrito_Rengar
{
    class Combo
    {
        private static void Game_OnUpdate(EventArgs args)
        {
            ComboLogic();
        }
        public static void ComboLogic()
        {
            var target = TargetSelector.GetSelectedTarget();
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                if (Program.Player.Mana <= 4)
                {
                    if (Spells._q.IsReady())
                        Spells._q.Cast(target);

                    if (Spells._w.IsReady())
                    {
                        Spells._w.Cast(target);
                        Logic.CastHydra();
                    }
                    if (Spells._e.IsReady())
                        Spells._e.Cast(target);
                }
                if (Program.Player.Mana == 5)
                {
                    if (Spells._q.IsReady())
                        Spells._q.Cast(target);
                    if (Spells._w.IsReady())
                    {
                        Spells._w.Cast(target);
                        Logic.CastHydra();
                    }
                    if (Spells._e.IsReady())
                        Spells._e.Cast(target);
                    if (Spells._e.IsReady())
                        Spells._e.Cast(target);
                }
            }
        }
        
    }
}
