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
                if (Player.Mana == 5 && (Player.Distance(target.Position) <= 850f))
                {
                    if (Spells._q.IsReady())
                    {
                        Spells._q.Cast(target);
                        Spells._e.Cast(target);
                    }
                    CastYoumoo();
                    if (Smite != SpellSlot.Unknown
                   && Player.Spellbook.CanUseSpell(Smite) == SpellState.Ready && !target.IsZombie)
                        Player.Spellbook.CastSpell(Smite, target);
                    if ((Player.Distance(target.Position) <= 170f))
                        CastHydra();
                    if (Spells._w.IsReady())   
                        Spells._w.Cast(target);
                    
                    if (Player.Mana <= 4)
                    {
                        if (Spells._e.IsReady())
                            Spells._e.Cast(target);
                        if (Spells._q.IsReady())
                            Spells._q.Cast(target);
                            CastYoumoo();
                        if (Smite != SpellSlot.Unknown
                       && Player.Spellbook.CanUseSpell(Smite) == SpellState.Ready && !target.IsZombie)
                        {
                            Player.Spellbook.CastSpell(Smite, target);
                        }
                        if ((Player.Distance(target.Position) <= 170f))
                            CastHydra();
                        if (Spells._w.IsReady())
                            Spells._w.Cast(target);

                    }
                }
            }
        }
    }
}
