using System;
using LeagueSharp.Common;

namespace Nechrito_Rengar
{
    class TripleQ : Logic
    {
        private static void Game_OnUpdate(EventArgs args)
        {
            TripleQLogic();
        }
        public static void TripleQLogic()
        {
            var target = TargetSelector.GetTarget(375f + Player.AttackRange + 70, TargetSelector.DamageType.Physical);
            {
                if (target != null && target.IsValidTarget() && !target.IsZombie)
                {
                    if (Player.Mana == 5)
                    {
                        if (Spells._q.IsReady())
                        {
                            Spells._q.Cast(target);
                            CastHydra();
                        }
                    }
                    if (Player.Mana <= 4)
                    {

                        if (Spells._q.IsReady())
                            Spells._q.Cast(target);
                        if (Spells._w.IsReady())
                        {
                            CastHydra();
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
