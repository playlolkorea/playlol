using System;
using LeagueSharp.Common;
using LeagueSharp;

namespace Nechrito_Rengar
{
    class Burst : Logic
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
                if (Player.Mana == 5 && target.IsValidTarget(Spells._q.Range))
                {
                    CastYoumoo();
                    Spells._q.Cast(target);
                    Player.Spellbook.CastSpell(Smite, target);
                    Spells._e.Cast(target);
                    CastHydra();
                    Spells._w.Cast(target);
                }
                if (Player.Mana == 4 && target.IsValidTarget(Spells._q.Range))
                {
                    CastYoumoo();
                    Spells._e.Cast(target);
                    CastHydra();
                    Spells._q.Cast(target);
                    Player.Spellbook.CastSpell(Smite, target);
                    Spells._w.Cast(target);
                }
                if (Player.Mana <= 3 && target.IsValidTarget(Spells._q.Range))
                {
                    Spells._w.Cast(target);
                    CastHydra();
                    CastYoumoo();
                    Spells._e.Cast(target);
                    Spells._q.Cast(target);
                    Player.Spellbook.CastSpell(Smite, target);

                }
            }
        }
    }
}
