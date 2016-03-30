using System;
using LeagueSharp.Common;

namespace NechritoRiven
{
    class Harass
    {
        private static void Game_OnUpdate(EventArgs args)
        {
            HarassLogic();
        }
       public static void HarassLogic()
        {
            var target = TargetSelector.GetTarget(400, TargetSelector.DamageType.Physical);
            if (Spells._q.IsReady() && Spells._w.IsReady() && Spells._e.IsReady() && Program._qstack == 1)
            {
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    Logic.ForceCastQ(target);
                    Utility.DelayAction.Add(1, Logic.ForceW);
                }
            }
            if (Spells._q.IsReady() && Spells._e.IsReady() && Program._qstack == 3 && !Orbwalking.CanAttack() && Orbwalking.CanMove(5))
            {
                var epos = Program.Player.ServerPosition +
                           (Program.Player.ServerPosition - target.ServerPosition).Normalized() * 300;
                Spells._e.Cast(epos);
                Utility.DelayAction.Add(190, () => Spells._q.Cast(epos));
            }
        }
    }
}
