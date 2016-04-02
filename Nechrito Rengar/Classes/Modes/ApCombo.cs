using System;
using LeagueSharp.Common;
using LeagueSharp;

namespace Nechrito_Rengar
{
    class ApCombo : Logic
    {
        private static void Game_OnUpdate(EventArgs args)
        {
            ApComboLogic();
        }
        public static void ApComboLogic()
        {
            var target = TargetSelector.GetTarget(Spells._e.Range - 80, TargetSelector.DamageType.Physical);
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                if (Player.Mana == 5)
                {
                    if (Spells._q.IsReady())
                        Spells._q.Cast(target);
                    if (Spells._w.IsReady())
                        Spells._w.Cast(target);
                    if (Smite != SpellSlot.Unknown
                   && Player.Spellbook.CanUseSpell(Smite) == SpellState.Ready && !target.IsZombie)
                    {
                        Player.Spellbook.CastSpell(Smite, target);
                    }
                    if (Spells._w.IsReady())
                        Spells._w.Cast(target);
                    if (Spells._e.IsReady())
                        Spells._e.Cast(target);
                }
                if (Player.Mana <= 4)
                {
                    if (Spells._q.IsReady())
                        Spells._q.Cast(target);
                    if (Smite != SpellSlot.Unknown
                   && Player.Spellbook.CanUseSpell(Smite) == SpellState.Ready && !target.IsZombie)
                    {
                        Player.Spellbook.CastSpell(Smite, target);
                    }
                    if (Spells._w.IsReady())
                        Spells._w.Cast(target);
                    if (Spells._w.IsReady())
                        Spells._w.Cast(target);
                    if (Spells._e.IsReady())
                        Spells._e.Cast(target);
                    
                   
                }
            }
        }

    }
}
