using System;
using LeagueSharp.Common;

namespace Nechrito_Rengar
{
    class Harass
    {
        private static void Game_OnUpdate(EventArgs args)
        {
            HarassLogic();
        }
        public static void HarassLogic()
        {
            var target = TargetSelector.GetTarget(375f + Program.Player.AttackRange + 70, TargetSelector.DamageType.Physical);
            {
                if (target != null && target.IsValidTarget() && !target.IsZombie)
                {
                    if (Program.Player.Mana == 5)
                    {
                        if (Spells._q.IsReady())
                        {
                            Spells._q.Cast(target);
                            Logic.CastHydra();
                        }
                        if (Spells._q.IsReady())
                            Spells._q.Cast(target);
                        if (Spells._q.IsReady())
                            Spells._q.Cast(target);
                    }
                    if (Program.Player.Mana <= 4)
                    {
                        if (Spells._e.IsReady())
                            Spells._e.Cast(target);

                        if (Spells._q.IsReady())
                            Spells._q.Cast(target);

                        if (Spells._w.IsReady())
                        {
                            Logic.CastHydra();
                            Spells._w.Cast(target);
                        }
                       
                    }

                }
            }
        }
    }
}
