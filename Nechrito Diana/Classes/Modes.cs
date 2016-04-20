using LeagueSharp.Common;
using LeagueSharp;
using SharpDX;

namespace Nechrito_Diana
{
    class Modes
    {
        public static void ComboLogic()
        {
            var target = TargetSelector.GetTarget(900, TargetSelector.DamageType.Magical);
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                    if (Spells._q.IsReady() && (Program.Player.Distance(target.Position) <= 700f))
                        Spells._q.Cast(target);
                    if (Spells._r.IsReady())
                        Spells._r.Cast(target);
                    if(Spells._w.IsReady())
                        Spells._w.Cast(target);
                    if (Spells._e.IsReady())
                        Spells._e.Cast(target);  
            }
        }
        public static void JungleLogic()
        {
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                var mobs = MinionManager.GetMinions(800 + Program.Player.AttackRange, MinionTypes.All, MinionTeam.Neutral,
           MinionOrderTypes.MaxHealth);
                if (mobs.Count <= 0)
                    return;
                if (Spells._q.IsReady() && Spells._r.IsReady() && (Program.Player.Distance(mobs[0].Position) <= 750f) && (Program.Player.Distance(mobs[0].Position) >= 600f) && MenuConfig.jnglQR)
                {
                    Spells._q.Cast(mobs[0].ServerPosition);
                    Spells._r.Cast(mobs[0].ServerPosition);
                }
                else if (Spells._w.IsReady() && (Program.Player.Distance(mobs[0].Position) <= 300f) && MenuConfig.jnglW)
                    Spells._w.Cast(mobs[0].ServerPosition);

                if (Program.Player.IsRecalling())
                    return;
                if(Spells._e.IsReady())
                {
                    var minion = MinionManager.GetMinions(Program.Player.Position, Spells._e.Range);
                    foreach (var m in mobs)
                    {
                        if (m.IsAttackingPlayer && Program.Player.HealthPercent <= MenuConfig.jnglE.MaxValue)
                            Spells._e.Cast(m);
                    }
                }
            }
        }
    }
}
