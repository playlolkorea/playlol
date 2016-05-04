using LeagueSharp;
using LeagueSharp.Common;
using System;
using System.Linq;

namespace NechritoRiven
{
    class Modes : Riven
    {
        public static void Load()
        {
            Game.OnUpdate += OnUpdate;
        }
        private static void OnUpdate(EventArgs args)
        {
            ComboLogic();
            JungleLogic();
            BurstLogic();
            HarassLogic();
            LaneLogic();
            JungleLogic();
            FastHarassLogic();
            FleeLogic();
        }
        #region Burst
        public static void BurstLogic()
        {

            var target = TargetSelector.GetSelectedTarget();
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                // Flash
                if (target.Health < Dmg.Totaldame(target) || MenuConfig.AlwaysF)
                {
                    if ((Player.Distance(target.Position) <= 700) && (Player.Distance(target.Position) >= 600) && Player.Spellbook.GetSpell(Spells.Flash).IsReady() && Spells._r.IsReady() && Spells._e.IsReady() && Spells._w.IsReady())
                    {
                        Spells._e.Cast(target.Position);
                        
                        Logic.CastYoumoo();
                        Utility.DelayAction.Add(10, () => Player.Spellbook.CastSpell(Spells.Flash, target.Position));
                        Utility.DelayAction.Add(20, Logic.CastHydra);
                        Utility.DelayAction.Add(150, Logic.ForceW);
                        Spells._r.Cast(target.ServerPosition);
                        Utility.DelayAction.Add(160, () => Logic.ForceCastQ(target));
                    }
                }

                // Burst
                else if ((Player.Distance(target.Position) <= Spells._e.Range + Player.AttackRange + Player.BoundingRadius - 20))
                {
                    if (Spells._e.IsReady())
                    { Spells._e.Cast(target.Position); }
                    Logic.CastYoumoo();
                    if (Spells._r.IsReady() && Spells._r.Instance.Name == IsFirstR)
                    {
                        Spells._r.Cast();
                    }
                    Utility.DelayAction.Add(160, Logic.ForceW);
                    Utility.DelayAction.Add(70, Logic.CastHydra);
                    if (Spells._r.IsReady() && Spells._r.Instance.Name == IsSecondR)
                    { Spells._r.Cast(target.ServerPosition); }
                    if (Spells._q.IsReady())
                    { Utility.DelayAction.Add(170, () => Logic.ForceCastQ(target)); }

                }
                if ((Player.Distance(target.Position) <= Spells._e.Range))
                {
                        Logic.CastHydra();
                       
                    if (Spells._e.IsReady())
                    {
                        Spells._e.Cast(target.Position);
                    }
                    if (Spells._w.IsReady())
                    {
                        Spells._w.Cast(target.Position);
                    }
                    if (Spells._r.IsReady() && Spells._r.Instance.Name == IsSecondR)
                    {
                        Spells._r.Cast(target);
                    }
                    else if (Spells._q.IsReady())
                    {
                       
                        Utility.DelayAction.Add(170, () => Logic.ForceCastQ(target));
                    }
                }
            }
        }
        #endregion Burst
        #region Combo
        public static void ComboLogic()
        {
            {
                var targetR = TargetSelector.GetTarget(250 + Player.AttackRange + 70, TargetSelector.DamageType.Physical);
                if (Spells._r.IsReady() && Spells._r.Instance.Name == IsFirstR && MenuConfig.AlwaysR &&
                    targetR != null) Spells._r.Cast();

                if (Spells._w.IsReady() && Logic.InWRange(targetR) && targetR != null) Spells._w.Cast();
                if (Spells._r.IsReady() && Spells._r.Instance.Name == IsFirstR && Spells._w.IsReady() && targetR != null &&
                    Spells._e.IsReady() &&
                    targetR.IsValidTarget() && !targetR.IsZombie && (Dmg.IsKillableR(targetR) || MenuConfig.AlwaysR))
                {
                    if (!Logic.InWRange(targetR))
                    {
                        Spells._e.Cast(targetR.Position);
                        Spells._r.Cast();
                        Utility.DelayAction.Add(200, Logic.ForceW);
                        Utility.DelayAction.Add(30, () => Logic.ForceCastQ(targetR));
                    }
                }

                else if (Spells._w.IsReady() && Spells._e.IsReady())
                {
                    if (targetR.IsValidTarget() && targetR != null && !targetR.IsZombie && !Logic.InWRange(targetR))
                    {
                        Spells._e.Cast(targetR.Position);
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
        #endregion
        #region FastHarass
        public static void FastHarassLogic()
        {
            var target = TargetSelector.GetTarget(400, TargetSelector.DamageType.Physical);
            if (Spells._q.IsReady() && Spells._w.IsReady() && _qstack == 1)
            {
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    Logic.ForceCastQ(target);
                    Utility.DelayAction.Add(1, Logic.ForceW);
                }
            }
            if (Spells._q.IsReady() && _qstack == 3)
            {
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    Logic.ForceCastQ(target);
                    Utility.DelayAction.Add(1, Logic.ForceW);
                }
            }
            Logic.CastHydra();
            if (Spells._w.IsReady() && Logic.InWRange(target))
            {

                Utility.DelayAction.Add(1, Logic.ForceW);
                Utility.DelayAction.Add(2, () => Logic.ForceCastQ(target));
            }
            else if (Spells._q.IsReady())
            {

                Utility.DelayAction.Add(1, () => Logic.ForceCastQ(target));
            }
            else if (Spells._e.IsReady() && !Orbwalking.InAutoAttackRange(target) && !Logic.InWRange(target))
            {
                Spells._e.Cast(target.Position);
            }
        }
        #endregion
        #region Flee
        public static void FleeLogic()
        {
           if(MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Flee)
            {
                var enemy =
           HeroManager.Enemies.Where(
               hero =>
                   hero.IsValidTarget(Player.HasBuff("RivenFengShuiEngine")
                       ? 70 + 195 + Player.BoundingRadius
                       : 70 + 120 + Player.BoundingRadius) && Spells._w.IsReady());
                var x = Player.Position.Extend(Game.CursorPos, 300);
                var objAiHeroes = enemy as Obj_AI_Hero[] ?? enemy.ToArray();
                if (Spells._q.IsReady() && !Player.IsDashing()) Spells._q.Cast(Game.CursorPos);
                if (Spells._w.IsReady() && objAiHeroes.Any()) foreach (var target in objAiHeroes) if (Logic.InWRange(target)) Spells._w.Cast();
                if (Spells._e.IsReady() && !Player.IsDashing()) Spells._e.Cast(x);
            }
            /*
            var jump = JumpPos.Where(x => x.Value.Distance(Player.Position) < 300f && x.Value.Distance(Game.CursorPos) < 300f).FirstOrDefault();
            if(Spells._q.IsReady() && _qstack < 3)
            { Spells._q.Cast(); }
            if (jump.Value.IsValid())
            {
                ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, jump.Value);
                foreach (var pos in JumpPos)
                {
                    if (Game.CursorPos.Distance(pos.Value) <= 350 && ObjectManager.Player.Position.Distance(pos.Value) <= 900 && Spells._q.IsReady() && Spells._e.IsReady() && _qstack == 3)
                    {
                        Spells._e.Cast(pos.Value);
                        Spells._q.Cast(pos.Value);
                    }
                    else if (Game.CursorPos.Distance(pos.Value) <= 350 && ObjectManager.Player.Position.Distance(pos.Value) <= 900 && Spells._q.IsReady() && !Spells._e.IsReady() && _qstack == 3)
                    {
                        Spells._q.Cast(pos.Value);
                    }
                }
            }
            */
            //if (!MenuConfig.WallFlee)
            // {
            //  }
        }
        #endregion
        #region Harass
        public static void HarassLogic()
        {
            var target = TargetSelector.GetTarget(400, TargetSelector.DamageType.Physical);
            if (Spells._q.IsReady() && Spells._w.IsReady() && Spells._e.IsReady() && _qstack == 1)
            {
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    Logic.ForceCastQ(target);
                    Utility.DelayAction.Add(1, Logic.ForceW);
                }
            }
            if (_qstack == 2 && Spells._q.IsReady())
            {

                Utility.DelayAction.Add(1, () => Logic.ForceCastQ(target));
            }
            if (Spells._q.IsReady() && Spells._e.IsReady() && _qstack == 3 && !Orbwalking.CanAttack() && Orbwalking.CanMove(5))
            {
                var epos = Player.ServerPosition +
                          (Player.ServerPosition - target.ServerPosition).Normalized() * 300;
                Spells._e.Cast(epos);
                Utility.DelayAction.Add(190, () => Spells._q.Cast(epos));
            }
        }
        #endregion
        #region Lane
        public static void JungleLogic()
        {
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                var minions = MinionManager.GetMinions(800f).FirstOrDefault();
                if (minions == null)
                    return;

                if (Spells._e.IsReady() && MenuConfig.LaneE)
                    Spells._e.Cast(minions.ServerPosition);

                if (Spells._q.IsReady() && MenuConfig.LaneQ && !MenuConfig.FastC)
                {
                    Spells._q.Cast(Logic.GetCenterMinion());
                    Logic.CastHydra();
                }
                if (Spells._q.IsReady() && MenuConfig.LaneQ && MenuConfig.FastC)
                {
                    Logic.CastHydra();
                    Utility.DelayAction.Add(1, () => Logic.ForceCastQ(minions));
                }

                if (Spells._w.IsReady() && MenuConfig.LaneW)
                {
                    var minion = MinionManager.GetMinions(Player.Position, Spells._w.Range);
                    foreach (var m in minion)
                    {
                        if (m.Health < Spells._w.GetDamage(m) && minion.Count > 2)
                            Spells._w.Cast(m);
                        if (m.Health < Spells._q.GetDamage(m) && minion.Count > 1 && _qstack < 3)
                            Spells._q.Cast(m);
                    }
                }

                if (Spells._e.IsReady() && !Orbwalking.InAutoAttackRange(minions))
                {
                    Spells._e.Cast(minions.Position);
                }
            }
        }
        #endregion
        #region Jungle
        public static void LaneLogic()
        {
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                var mobs = MinionManager.GetMinions(Player.Position, 600f, MinionTypes.All,
                    MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();
                if (mobs == null)
                    return;

                // JUNGLE
                if (Spells._e.IsReady() && !Orbwalking.InAutoAttackRange(Logic.GetCenterMinion()) && MenuConfig.jnglE)
                {
                    Spells._e.Cast(mobs);
                }
                
                if (Spells._q.IsReady() && MenuConfig.jnglQ)
                {
                    
                    Logic.CastHydra();
                    Spells._q.Cast(mobs);
                }
                if (Spells._w.IsReady() && MenuConfig.jnglW)
                {
                    
                    Spells._w.Cast(mobs);
                }
            }
        }
        #endregion
    }
}
