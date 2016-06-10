﻿#region

using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using NechritoRiven.Core;
using NechritoRiven.Menus;

#endregion

namespace NechritoRiven.Event
{
    internal class Modes : Core.Core
    {
        // Laneclear
        public static void OnDoCastLc(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || !Orbwalking.IsAutoAttack(args.SData.Name)) return;
            qTarget = (Obj_AI_Base) args.Target;
            if (args.Target is Obj_AI_Minion)
            {
                if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
                {
                    var minions = MinionManager.GetMinions(70 + 120 + Player.BoundingRadius);
                    if (minions.Count < 1) return;

                    if (!Spells.Q.IsReady() || !MenuConfig.LaneQ) return;

                    foreach (var m in minions)
                    {
                        ForceCastQ(m);
                        Usables.CastHydra();
                    }
                    
                }
            }
        }

        // Jungle, Combo etc.
        public static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var spellName = args.SData.Name;
            if (!sender.IsMe || !Orbwalking.IsAutoAttack(spellName)) return;
            qTarget = (Obj_AI_Base)args.Target;

            if (args.Target is Obj_AI_Minion)
            {
                Jungleclear();
                if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
                {
                    var minions = MinionManager.GetMinions(600f).FirstOrDefault();
                    if (minions == null)
                        return;

                   
                    if (Spells.E.IsReady() && MenuConfig.LaneE)
                        Spells.E.Cast(minions.ServerPosition);

                    if (Spells.W.IsReady() && MenuConfig.LaneW)
                    {
                        var minion = MinionManager.GetMinions(Player.Position, Spells.W.Range);
                        foreach (var m in minion)
                        {
                            if (m.Health < Spells.W.GetDamage(m) && minion.Count > 2)
                                Utility.DelayAction.Add(200, () => Spells.W.Cast(m));
                        }
                    }
                }
                
            }

            var @base = args.Target as Obj_AI_Turret;
            if (@base != null)
            {
                if (@base.IsValid && args.Target != null && Spells.Q.IsReady() && MenuConfig.LaneQ &&
                    _orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear) ForceCastQ(@base);
            }


            var hero = args.Target as Obj_AI_Hero;
            if (hero == null) return;
            var target = hero;

            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                
                if (Spells.E.IsReady())
                {
                    Spells.E.Cast(target.Position);
                    Usables.CastHydra();
                }

                if (Spells.W.IsReady() && InWRange(target))
                {
                    Usables.CastHydra();
                    Spells.W.Cast();
                }

                if (Spells.Q.IsReady())
                {
                    ForceItem();
                    Utility.DelayAction.Add(1, () => ForceCastQ(target));
                }

                if (Spells.R.IsReady() && Qstack > 2 && !MenuConfig.OverKillCheck && Spells.R.Instance.Name == IsSecondR)
                    Spells.R.Cast(target.Position);

                else if (MenuConfig.OverKillCheck && Spells.R.IsReady() && Spells.R.Instance.Name == IsSecondR && !Spells.Q.IsReady())
                    Spells.R.Cast(target.Position);
            }

            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
              {

                if (Qstack == 2 && Spells.Q.IsReady())
                {
                    Usables.CastHydra();
                    ForceItem();
                    Utility.DelayAction.Add(1, () => ForceCastQ(target));
                }
              }

            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Burst)
            {

                if (Spells.E.IsReady() && (Player.Distance(target.Position) <= Spells.E.Range + Player.AttackRange))
                    Spells.E.Cast(target);

                if (Player.Distance(target.Position) <= Spells.W.Range)
                {
                    if (Spells.W.IsReady())
                    {
                        Spells.W.Cast(target);
                    }
                    if (Spells.Q.IsReady())
                    {
                        Utility.DelayAction.Add(30, () => ForceCastQ(Target));
                        Usables.CastHydra();
                    }
                }
                if (Spells.R.IsReady() && Qstack >= 2 && Spells.R.Instance.Name == IsSecondR)
                    Spells.R.Cast(target.Position);
            }
        }

        public static void QMove()
        {

            if (!MenuConfig.QMove)
            {
                return;
            }

            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            if (Spells.Q.IsReady())
            {
                Utility.DelayAction.Add(47, () => Spells.Q.Cast(Game.CursorPos));
            }
           

        }

        public static void Jungleclear()
        {
            if (_orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear) return;

            var mobs = MinionManager.GetMinions(Player.Position, 600f, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();

            if (mobs?.Distance(Player.Position) <= Spells.E.Range + Player.AttackRange)
            {
                if (Spells.E.IsReady() && MenuConfig.jnglE)
                {
                    Spells.E.Cast(mobs);
                    Usables.CastHydra();
                }

                if (Spells.Q.IsReady() && MenuConfig.jnglQ)
                {
                    Usables.CastHydra();
                    Utility.DelayAction.Add(1, () => ForceCastQ(mobs));
                }

                if (!Spells.W.IsReady() || !MenuConfig.jnglW) return;
                Usables.CastHydra();
                Utility.DelayAction.Add(150, () => Spells.W.Cast(mobs));
            }
        }

        public static void Combo()
        {
            if(Target == null || Target.IsDead || !Target.IsValidTarget() || Target.IsInvulnerable) return;

            if (Spells.R.IsReady() && Spells.R.Instance.Name == IsFirstR && MenuConfig.AlwaysR) ForceR();

            if (Spells.W.IsReady() && InWRange(Target)) Spells.W.Cast();

            if (Spells.R.IsReady() && Spells.R.Instance.Name == IsFirstR && Spells.W.IsReady() &&
                Spells.E.IsReady() && (Dmg.IsKillableR(Target) || MenuConfig.AlwaysR))
            {
                if (InWRange(Target)) return;

                Spells.E.Cast(Target.Position);
                ForceR();
                Utility.DelayAction.Add(170, ForceW);
                Utility.DelayAction.Add(30, () => ForceCastQ(Target));
            }

            else if (Spells.W.IsReady() && Spells.E.IsReady())
            {
                Spells.E.Cast(Target.Position);
                if (!InWRange(Target)) return;

                Utility.DelayAction.Add(100, ForceW);
                Utility.DelayAction.Add(30, () => ForceCastQ(Target));
            }
            else if (Spells.E.IsReady())
            {
                if (!InWRange(Target))
                {
                    Spells.E.Cast(Target.Position);
                }
            }
        }

        public static void Burst()
        {
            var target = TargetSelector.GetSelectedTarget();

            if (target == null || !target.IsValidTarget() || target.IsZombie || target.IsInvulnerable) return;

            if(Spells.Flash.IsReady())
            {
                if (target.Health < Dmg.Totaldame(target) || MenuConfig.AlwaysF)
                {
                    if (Player.Distance(target.Position) <= 700 && Player.Distance(target.Position) >= 600)
                    {
                        if (Spells.R.IsReady() && Spells.E.IsReady() && Spells.W.IsReady() && Spells.R.Instance.Name == IsFirstR)
                        {
                            Spells.E.Cast(target.Position);
                            Usables.CastYoumoo();
                            ForceR();
                            Utility.DelayAction.Add(180, FlashW);
                            Usables.CastHydra();
                        }
                    }
                }
            }
            else
            {
                if (Player.Distance(target) <= Spells.E.Range + Player.BoundingRadius)
                {
                    if (Spells.E.IsReady() && Spells.R.IsReady())
                    {
                        Spells.E.Cast(Target.ServerPosition);
                        ForceR();
                    }
                    else if (Spells.E.IsReady())
                    {
                        Spells.E.Cast(Target);
                    }
                }
            }
        }

        public static void FastHarass()
        {
            if (Spells.Q.IsReady() && Spells.E.IsReady())
            {
                var target = TargetSelector.GetTarget(450 + Player.AttackRange + 70, TargetSelector.DamageType.Physical);
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    if (!Orbwalking.InAutoAttackRange(target) && !InWRange(target)) Spells.E.Cast(target.Position);
                    Utility.DelayAction.Add(10, ForceItem);
                    Utility.DelayAction.Add(170, () => ForceCastQ(target));
                }
            }
        }

        public static void Harass()
        {
            var target = TargetSelector.GetTarget(400, TargetSelector.DamageType.Physical);
            if (Spells.Q.IsReady() && Spells.W.IsReady() && Spells.E.IsReady() && Qstack == 1)
            {
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    ForceCastQ(target);
                    Utility.DelayAction.Add(1, ForceW);
                }
            }
            if (Spells.Q.IsReady() && Spells.E.IsReady() && Qstack == 3 && !Orbwalking.CanAttack() && Orbwalking.CanMove(5))
            {
                var epos = Player.ServerPosition + (Player.ServerPosition - target.ServerPosition).Normalized() * 300;
                Spells.E.Cast(epos);
                Utility.DelayAction.Add(190, () => Spells.Q.Cast(epos));
            }
        }

        public static void Flee()
        {
            if (MenuConfig.WallFlee)
            {
                var end = Player.ServerPosition.Extend(Game.CursorPos, Spells.Q.Range);
                var IsWallDash = FleeLogic.IsWallDash(end, Spells.Q.Range);

                var Eend = Player.ServerPosition.Extend(Game.CursorPos, Spells.E.Range);
                var WallE = FleeLogic.GetFirstWallPoint(Player.ServerPosition, Eend);
                var WallPoint = FleeLogic.GetFirstWallPoint(Player.ServerPosition, end);
                Player.GetPath(WallPoint);

                if (Spells.Q.IsReady() && Qstack < 3)
                { Spells.Q.Cast(Game.CursorPos); }


                if (IsWallDash && Qstack == 3 && WallPoint.Distance(Player.ServerPosition) <= 800)
                {
                    ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, WallPoint);
                    if (WallPoint.Distance(Player.ServerPosition) <= 600)
                    {
                        ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, WallPoint);
                        if (WallPoint.Distance(Player.ServerPosition) <= 45)
                        {
                            if (Spells.E.IsReady())
                            {
                                Spells.E.Cast(WallE);
                            }
                            if (Qstack == 3 && end.Distance(Player.Position) <= 260 && IsWallDash && WallPoint.IsValid())
                            {
                                Player.IssueOrder(GameObjectOrder.MoveTo, WallPoint);
                                Spells.Q.Cast(WallPoint);
                            }

                        }
                    }
                }
            }
            else
            {
                var enemy = HeroManager.Enemies.Where(hero => hero.IsValidTarget(Player.HasBuff("RivenFengShuiEngine")
                           ? 70 + 195 + Player.BoundingRadius
                           : 70 + 120 + Player.BoundingRadius) && Spells.W.IsReady());

                var x = Player.Position.Extend(Game.CursorPos, 300);
                var objAiHeroes = enemy as Obj_AI_Hero[] ?? enemy.ToArray();
                if (Spells.W.IsReady() && objAiHeroes.Any()) foreach (var target in objAiHeroes) if (InWRange(target)) Spells.W.Cast();
                if (Spells.Q.IsReady() && !Player.IsDashing()) Spells.Q.Cast(Game.CursorPos);
                if (Spells.E.IsReady() && !Player.IsDashing()) Spells.E.Cast(x);
            }
        }
    }
}
