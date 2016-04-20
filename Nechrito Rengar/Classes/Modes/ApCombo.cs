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
                        }
                        else if (Spells._e.IsReady() && (Player.Distance(target.Position) <= 300))
                        {

                            Spells._e.Cast(target);
                        }

                        else if (Spells._w.IsReady() && (Player.Distance(target.Position) <= 270))
                        {
                            CastHydra();
                            Spells._w.Cast(target);
                        }

                        else if (Spells._q.IsReady())
                        {
                            CastHydra();
                            Spells._q.Cast(target);
                        }
                    }
                    if (Player.Mana <= 4)
                    {

                        if (Spells._q.IsReady())
                        {
                           
                            Spells._q.Cast(target);
                        }

                        else if (Spells._e.IsReady() && (Player.Distance(target.Position) <= 300))
                        {

                            Spells._e.Cast(target);
                        }

                        else if (Spells._w.IsReady() && (Player.Distance(target.Position) <= 270))
                        {
                            CastHydra();
                            Spells._w.Cast(target);
                        }

                        else if (Spells._q.IsReady())
                        {
                            CastHydra();
                            Spells._q.Cast(target);
                        }
                    }

                }
            }
        }
    }
}
