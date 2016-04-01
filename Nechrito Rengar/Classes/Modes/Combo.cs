using System;
using LeagueSharp.Common;

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
            var target = TargetSelector.GetTarget(Spells._e.Range, TargetSelector.DamageType.Physical);
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                if (Program.Player.Mana == 5)
                {
                    if (Spells._e.IsReady())
                        Spells._e.Cast(target);
                       Logic.CastYoumoo();
                    if (Spells._e.IsReady())
                        Spells._e.Cast(target);
                    if (Spells._q.IsReady() && !Spells._e.IsReady() && (Program.Player.Distance(target.Position) <= Spells._q.Range))
                    {
                        Spells._q.Cast(target);
                        Logic.CastHydra();
                        Logic.SmiteLogic();
                    }
                    if (Spells._w.IsReady())
                        Spells._w.Cast(target);
                    
                }
                if (Program.Player.Mana <= 4)
                {
                    if (Spells._q.IsReady() && (Program.Player.Distance(target.Position) <= Spells._q.Range))
                    {
                        Spells._q.Cast(target);
                        Logic.CastHydra();
                    }
                    if (Spells._w.IsReady() && (Program.Player.Distance(target.Position) <= Spells._q.Range))
                        Spells._w.Cast(target);
                    
                    if (Spells._e.IsReady())
                        Spells._e.Cast(target);
                }
               
            }
        }
        
    }
}
