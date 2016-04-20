using System;
using LeagueSharp.Common;
using LeagueSharp;

namespace Nechrito_Rengar
{
    class Combo : Logic
    {
        private static void Game_OnUpdate(EventArgs args)
        {
            ComboLogic();
        }
        public static void ComboLogic()
        {
            var target = TargetSelector.GetTarget(Spells._e.Range - 80, TargetSelector.DamageType.Physical);
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                if (Player.Mana == 5)
                {
                    if (Spells._e.IsReady())
                        Spells._e.Cast(target);
                       CastYoumoo();
                    if (Spells._e.IsReady())
                        Spells._e.Cast(target);
                    if (Smite != SpellSlot.Unknown
                    && Player.Spellbook.CanUseSpell(Smite) == SpellState.Ready && !target.IsZombie)
                    {
                        Player.Spellbook.CastSpell(Smite, target);
                    }
                    if (Spells._q.IsReady() && !Spells._e.IsReady() && (Player.Distance(target.Position) <= Player.AttackRange + 30) && (target != null && target.IsValidTarget() && !target.IsZombie))
                        Spells._q.Cast(target);
                    if (Spells._w.IsReady() && (Player.Distance(target.Position) <= Player.AttackRange + 30) && (target != null && target.IsValidTarget() && !target.IsZombie))
                    {
                        CastHydra();
                        Spells._w.Cast(target);
                    }       
                }
                if (Player.Mana <= 4)
                {
                    if (Spells._q.IsReady() && (Player.Distance(target.Position) <= Player.AttackRange) && (target != null && target.IsValidTarget() && !target.IsZombie))
                        Spells._q.Cast(target);
                    if (Spells._e.IsReady() && (target != null && target.IsValidTarget() && !target.IsZombie))
                        Spells._e.Cast(target);
                    if (Spells._w.IsReady() && (Player.Distance(target.Position) <= Player.AttackRange + 30) && (target != null && target.IsValidTarget() && !target.IsZombie))
                    {
                        CastHydra();
                        Spells._w.Cast(target);
                    }
                }
            }
        }
    }
}
