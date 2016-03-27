using LeagueSharp.Common;

namespace NechritoRiven
{
    class Combo
    {
        public static void DoCombo()
        {
            
            var targetR = TargetSelector.GetTarget(250 + Program.Player.AttackRange + 70, TargetSelector.DamageType.Physical);

            if (Spells._r.IsReady() && Spells._r.Instance.Name == Program.IsFirstR && MenuConfig.AlwaysR &&
                targetR != null) Program.ForceR();
            if (Spells._r.IsReady() && Spells._r.Instance.Name == Program.IsFirstR && Spells._w.IsReady() && Program.InWRange(targetR) && MenuConfig.AlwaysR &&
                targetR != null)
            {
                Program.ForceR();
                Utility.DelayAction.Add(1, Program.ForceW);
            }

            if (Spells._w.IsReady() && Program.InWRange(targetR) && targetR != null) Spells._w.Cast();
            if (Spells._r.IsReady() && Spells._r.Instance.Name == Program.IsFirstR && Spells._w.IsReady() && targetR != null &&
                Spells._e.IsReady() &&
                targetR.IsValidTarget() && !targetR.IsZombie && (Program.IsKillableR(targetR) || MenuConfig.AlwaysR))
            {
                if (!Program.InWRange(targetR))
                {
                    Spells._e.Cast(targetR.Position);
                    Program.ForceR();
                    Utility.DelayAction.Add(20, Program.ForceW);
                    Utility.DelayAction.Add(0, () => Program.ForceCastQ(targetR));
                }
            }

            else if (Spells._w.IsReady() && Spells._e.IsReady())
            {
                if (targetR.IsValidTarget() && targetR != null && !targetR.IsZombie && !Program.InWRange(targetR))
                {
                    Spells._e.Cast(targetR.Position);
                    Utility.DelayAction.Add(0, Program.ForceItem);
                    if(Program.InWRange(targetR))
                    Utility.DelayAction.Add(0, Program.ForceW);
                    Utility.DelayAction.Add(0, () => Program.ForceCastQ(targetR));
                }
            }

            else if (Spells._e.IsReady())
            {
                if (targetR != null && (targetR.IsValidTarget() && !targetR.IsZombie && !Program.InWRange(targetR)))
                {
                    Spells._e.Cast(targetR.Position);
                }
            }
        }
    }
}
