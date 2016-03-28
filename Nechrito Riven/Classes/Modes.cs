
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace NechritoRiven
{
    class Modes
    {
        public static void DoCombo()
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
        public static void DoBurst()
        {
            var target = TargetSelector.GetSelectedTarget();

            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {

                // Else R wont cast, Bug
               // if (Spells._r.IsReady() && Spells._r.Instance.Name == Program.IsFirstR && MenuConfig.AlwaysR && Program.Player.Distance(target.Position) <= Spells._e.Range + (Program.Player.AttackRange) &&
               //   target != null)
                 //   Logic.ForceR();

                // Flash
                if ((Program.Player.Distance(target.Position) <= 700) && (Program.Player.Distance(target.Position) >= 600) && Program.Player.Spellbook.GetSpell(Spells.Flash).State == SpellState.Ready && Spells._r.IsReady() && Spells._e.IsReady() && Spells._w.IsReady() && target.Health < Dmg.Totaldame(target))
                {
                    Spells._e.Cast(target.Position);
                    Logic.ForceR();
                    Logic.CastYoumoo();
                    Utility.DelayAction.Add(65, () => Program.Player.Spellbook.CastSpell(Spells.Flash, target.Position));
                    Utility.DelayAction.Add(70, Logic.ForceItem);
                    Utility.DelayAction.Add(30, Logic.ForceW);
                    Spells._r.Cast(target.ServerPosition);
                    Utility.DelayAction.Add(30, () => Logic.ForceCastQ(target));
                }
                // Burst
                if (Spells._e.IsReady() && Spells._w.IsReady() &&
                        (Program.Player.Distance(target.Position) <= Spells._e.Range))
                {
                    Spells._e.Cast(target.ServerPosition);
                    Logic.ForceR();
                    Logic.CastYoumoo();
                    Utility.DelayAction.Add(70, Logic.ForceItem);
                    Utility.DelayAction.Add(50, Logic.ForceW);
                    Spells._r.Cast(target.ServerPosition);
                    Utility.DelayAction.Add(30, () => Logic.ForceCastQ(target));
                }

            }
        }

        public static void Jungleclear()
        {
            var mobs = MinionManager.GetMinions(190 + Program.Player.AttackRange, MinionTypes.All, MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth);

            if (mobs.Count <= 0)
                return;

            if (Spells._e.IsReady() && !Orbwalking.InAutoAttackRange(mobs[0]))
            {
                Spells._e.Cast(mobs[0].Position);
            }
        }
        public static void FastHarass()
        {
            var target = TargetSelector.GetTarget(400, TargetSelector.DamageType.Physical);
            if (Spells._q.IsReady() && Spells._w.IsReady() && Program._qstack == 1)
            {
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    Logic.ForceCastQ(target);
                    Utility.DelayAction.Add(1, Logic.ForceW);
                }
            }
            if (Spells._q.IsReady() && Program._qstack == 3)
            {
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    Logic.ForceCastQ(target);
                    Utility.DelayAction.Add(1, Logic.ForceW);
                }
            }
        }

        public static void Harass()
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

        public static void Flee()
        {
            var enemy =
                HeroManager.Enemies.Where(
                    hero =>
                        hero.IsValidTarget(Program.Player.HasBuff("RivenFengShuiEngine")
                            ? 70 + 195 + Program.Player.BoundingRadius
                            : 70 + 120 + Program.Player.BoundingRadius) && Spells._w.IsReady());
            var x = Program.Player.Position.Extend(Game.CursorPos, 300);
            var objAiHeroes = enemy as Obj_AI_Hero[] ?? enemy.ToArray();
            if (Spells._w.IsReady() && objAiHeroes.Any()) foreach (var target in objAiHeroes) if (Logic.InWRange(target)) Spells._w.Cast();
            if (Spells._q.IsReady() && !Program.Player.IsDashing()) Spells._q.Cast(Game.CursorPos);
            if (Spells._e.IsReady() && !Program.Player.IsDashing()) Spells._e.Cast(x);
        }

       

       

        public static Obj_AI_Base GetCenterMinion()
        {
            var minionposition = MinionManager.GetMinions(300 + Spells._q.Range).Select(x => x.Position.To2D()).ToList();
            var center = MinionManager.GetBestCircularFarmLocation(minionposition, 250, 300 + Spells._q.Range);

            return center.MinionsHit >= 3
                ? MinionManager.GetMinions(1000).OrderBy(x => x.Distance(center.Position)).FirstOrDefault()
                : null;
        }
    }
}
