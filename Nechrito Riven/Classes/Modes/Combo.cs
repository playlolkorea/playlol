using System;
using LeagueSharp.Common;

namespace NechritoRiven
{
    class Combo
    {
        private static void Game_OnUpdate(EventArgs args)
        {
            ComboLogic();
        }
        public static void ComboLogic()
        {
            {
                var targetR = TargetSelector.GetTarget(250 + Program.Player.AttackRange + 70, TargetSelector.DamageType.Physical);
                if (Spells._r.IsReady() && Spells._r.Instance.Name == Program.IsFirstR && MenuConfig.AlwaysR &&
                    targetR != null) Logic.ForceR();

                if (Spells._w.IsReady() && Logic.InWRange(targetR) && targetR != null) Spells._w.Cast();
                if (Spells._r.IsReady() && Spells._r.Instance.Name == Program.IsFirstR && Spells._w.IsReady() && targetR != null &&
                    Spells._e.IsReady() &&
                    targetR.IsValidTarget() && !targetR.IsZombie && (Dmg.IsKillableR(targetR) || MenuConfig.AlwaysR))
                {
                    if (!Logic.InWRange(targetR))
                    {
                        Spells._e.Cast(targetR.Position);
                        Logic.ForceR();
                        Utility.DelayAction.Add(200, Logic.ForceW);
                        Utility.DelayAction.Add(30, () => Logic.ForceCastQ(targetR));
                    }
                }

                else if (Spells._w.IsReady() && Spells._e.IsReady())
                {
                    if (targetR.IsValidTarget() && targetR != null && !targetR.IsZombie && !Logic.InWRange(targetR))
                    {
                        Spells._e.Cast(targetR.Position);
                        Utility.DelayAction.Add(10, Logic.ForceItem);
                        if (Logic.InWRange(targetR))
                        Utility.DelayAction.Add(100, Logic.ForceW);
                        Utility.DelayAction.Add(30, () => Logic.ForceCastQ(targetR));
                    }
                }
                else if (Spells._e.IsReady())
                {
                    if (targetR != null && (targetR.IsValidTarget() && !targetR.IsZombie && !Logic.InWRange(targetR)))
                    {
                        Spells._e.Cast(targetR.Position);
                    }
                }
            }
        }
    }
}
