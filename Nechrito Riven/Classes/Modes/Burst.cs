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
            if (target == null)
                return;
                // Flash
                if(target.Health < Dmg.Totaldame(target) || MenuConfig.AlwaysF)
                {
                    if ((Program.Player.Distance(target.Position) <= 700) && (Program.Player.Distance(target.Position) >= 600) && Program.Player.Spellbook.GetSpell(Spells.Flash).IsReady() && Spells._r.IsReady() && Spells._e.IsReady() && Spells._w.IsReady())
                    {
                    Spells._e.Cast(target.Position);
                    Utility.DelayAction.Add(10, () => Program.Player.Spellbook.CastSpell(Spells.Flash, target.Position));
                    Logic.ForceR();
                    Logic.CastYoumoo();
                    Utility.DelayAction.Add(70, Logic.ForceItem);
                    Utility.DelayAction.Add(70, Logic.CastHydra);
                    Utility.DelayAction.Add(150, Logic.ForceW);
                    Spells._r.Cast(target.ServerPosition);
                    Utility.DelayAction.Add(160, () => Logic.ForceCastQ(target));
                }
                }
               
                // Burst
                if ((Program.Player.Distance(target.Position) <= Spells._e.Range + Program.Player.AttackRange + Program.Player.BoundingRadius - 20))
                {
                Spells._e.Cast(target.Position);
                Logic.ForceR();
                Logic.CastYoumoo();
                Utility.DelayAction.Add(70, Logic.ForceItem);
                Utility.DelayAction.Add(70, Logic.CastHydra);
                Utility.DelayAction.Add(150, Logic.ForceW);
                Spells._r.Cast(target.ServerPosition);
                Utility.DelayAction.Add(160, () => Logic.ForceCastQ(target));
            }
        }
      }
    }