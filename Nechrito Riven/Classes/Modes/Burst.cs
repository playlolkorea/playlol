using System;
using LeagueSharp.Common;

namespace NechritoRiven
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
                if ((Program.Player.Distance(target.Position) <= 750) && (Program.Player.Distance(target.Position) >= 550) && Program.Player.Spellbook.GetSpell(Spells.Flash).IsReady() && Spells._r.IsReady() && Spells._e.IsReady() && Spells._w.IsReady() && MenuConfig.AlwaysF)
                {
                    Spells._e.Cast(target.Position);
                    Logic.ForceR();
                    Logic.CastYoumoo();
                    Utility.DelayAction.Add(35, () => Program.Player.Spellbook.CastSpell(Spells.Flash, target.Position));
                    Utility.DelayAction.Add(0, Logic.ForceItem);
                    Utility.DelayAction.Add(0, Program.CastTitan);
                    Utility.DelayAction.Add(0, Logic.ForceW);
                    Spells._r.Cast(target.ServerPosition);
                    Utility.DelayAction.Add(60, () => Logic.ForceCastQ(target));
                }
                // Flash
                if ((Program.Player.Distance(target.Position) <= 700) && (Program.Player.Distance(target.Position) >= 600) && Program.Player.Spellbook.GetSpell(Spells.Flash).IsReady() && Spells._r.IsReady() && Spells._e.IsReady() && Spells._w.IsReady() && target.Health < Dmg.Totaldame(target))
                {
                    Spells._e.Cast(target.Position);
                    Logic.ForceR();
                    Logic.CastYoumoo();
                    Utility.DelayAction.Add(35, () => Program.Player.Spellbook.CastSpell(Spells.Flash, target.Position));
                    Utility.DelayAction.Add(0, Logic.ForceItem);
                    Utility.DelayAction.Add(0, Program.CastTitan);
                    Utility.DelayAction.Add(0, Logic.ForceW);
                    Spells._r.Cast(target.ServerPosition);
                    Utility.DelayAction.Add(60, () => Logic.ForceCastQ(target));
                }
                // Burst
                if (Spells._e.IsReady() && Spells._w.IsReady() &&
                        (Program.Player.Distance(target.Position) <= Spells._e.Range + Program.Player.AttackRange - 20))
                {
                    Spells._e.Cast(target.ServerPosition);
                    Logic.ForceR();
                    Logic.CastYoumoo();
                    Logic.ForceItem();
                    Program.CastTitan();
                    Spells._w.Cast(target);
                    Utility.DelayAction.Add(60, () => Logic.ForceCastQ(target));
                    Spells._r.Cast(target.ServerPosition);
                    
                }
                
            }
        }
        
    }
}
