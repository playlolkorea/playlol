using System;
using LeagueSharp.Common;
using LeagueSharp;
using SharpDX;
using System.Collections.Generic;
using System.Linq;
using NechritoRiven.Classes.Extras;

namespace NechritoRiven.Classes.Modes
{
    class Modes : Core
    {
        public static void Combo()
        {
            var target = TargetSelector.GetTarget(250 + Player.AttackRange + 70, TargetSelector.DamageType.Physical);

            if (Orb.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                if (target != null && target.IsValidTarget() && !target.IsZombie)
                {
                    if (Spells.R.IsReady() && Spells.R.Instance.Name == Spells.IsFirstR && MenuConfig.AlwaysR)
                    {
                        Spells.R.Cast();
                    }
                    if (Spells.W.IsReady() && target.Distance(Player.Position) <= Spells.W.Range)
                    {
                        Spells.W.Cast(target);
                    }
                    if (Spells.Q.IsReady() && target.Distance(Player.Position) <= Spells.Q.Range)
                    {
                        Spells.Q.Cast(target);
                    }
                    if (Spells.R.IsReady() && Spells.R.Instance.Name == Spells.IsFirstR && Spells.W.IsReady() && Spells.E.IsReady() && Dmg.IsKillableR(target) || MenuConfig.AlwaysR)
                    {
                        Spells.E.Cast(target.Position);
                        Spells.R.Cast();
                        Utility.DelayAction.Add(200, Logic.ForceW);
                        Utility.DelayAction.Add(30, () => Logic.ForceQ(target));
                    }
                    else if (Spells.W.IsReady() && Spells.E.IsReady())
                    {
                        Spells.E.Cast(target.Position);
                        Utility.DelayAction.Add(100, Logic.ForceW);
                        Utility.DelayAction.Add(30, () => Logic.ForceQ(target));
                    }
                    else if (Spells.E.IsReady())
                    {
                        Spells.E.Cast(target);
                    }
                }
            }
        }

        public static void Game_OnUpdate(EventArgs args)
        {
            QMove();
        }

        public static void QMove()
        {
            
            if (!MenuConfig.QMove)
            {
                return;
            }

            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            Utility.DelayAction.Add(50,()=> Spells.Q.Cast(Game.CursorPos));
           
        }

        public static void Burst()
        {
            var target = TargetSelector.GetSelectedTarget();
            if (Orb.ActiveMode == Orbwalking.OrbwalkingMode.Burst)
            {
                if (target != null && target.IsValidTarget() && !target.IsZombie)
                {
                    if (target.Health < Dmg.Totaldame(target) || MenuConfig.AlwaysF)
                    {
                        if (Player.Distance(target.Position) <= 700 && Player.Distance(target.Position) >= 600)
                        {
                            ITEMS.CastYoumoo();
                            if (Spells.E.IsReady() && Spells.R.Instance.Name == Spells.IsFirstR)
                            {
                                Spells.E.Cast(target.Position);
                                Spells.R.Cast();
                            }
                            if (Player.Spellbook.GetSpell(Spells.Flash).IsReady())
                            {
                                Utility.DelayAction.Add(10,
                                    () => Player.Spellbook.CastSpell(Spells.Flash, target.Position));
                                if (Spells.W.IsReady())
                                {
                                    ITEMS.CastHydra();
                                    Spells.W.Cast();
                                }
                            }
                            if (Spells.W.IsReady())
                            {
                                Utility.DelayAction.Add(160, Logic.ForceW);
                            }
                            if (Spells.R.IsReady() && Spells.R.Instance.Name == Spells.IsSecondR)
                            {
                                Spells.R.Cast(target.ServerPosition);
                            }
                            if (Spells.Q.IsReady())
                            {
                                Utility.DelayAction.Add(150, () => Logic.ForceQ(target));
                            }
                        }
                    }
                    else
                    {
                        if (Spells.R.IsReady() && Spells.R.Instance.Name == Spells.IsFirstR && MenuConfig.AlwaysR)
                        {
                            Spells.R.Cast();
                        }
                        if (Spells.W.IsReady() && target.Distance(Player.Position) <= Spells.W.Range)
                        {
                            Spells.W.Cast(target);
                        }
                        if (Spells.R.IsReady() && Spells.R.Instance.Name == Spells.IsFirstR && Spells.W.IsReady() &&
                            Spells.E.IsReady() && Dmg.IsKillableR(target) || MenuConfig.AlwaysR)
                        {
                            Spells.E.Cast(target.Position);
                            Spells.R.Cast();
                            Utility.DelayAction.Add(200, Logic.ForceW);
                            Utility.DelayAction.Add(30, () => Logic.ForceQ(target));
                        }
                        else if (Spells.W.IsReady() && Spells.E.IsReady())
                        {
                            Spells.E.Cast(target.Position);
                            Utility.DelayAction.Add(100, Logic.ForceW);
                            Utility.DelayAction.Add(30, () => Logic.ForceQ(target));
                        }
                        else if (Spells.E.IsReady())
                        {
                            Spells.E.Cast(target);
                        }
                    }
                }
            }
        }

        public static void JungleClear()
        {

        }
        public static void LaneClear()
        {
            if (Orb.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                var mobs = MinionManager.GetMinions(Player.Position, 600f, MinionTypes.All,
                    MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();
                if (mobs == null)
                    return;

                // JUNGLE
                if (Spells.E.IsReady() && !Orbwalking.InAutoAttackRange(Logic.GetCenterMinion()) && MenuConfig.jnglE)
                {
                    ITEMS.CastHydra();
                    Spells.E.Cast(mobs);
                }
                if (Spells.Q.IsReady() && MenuConfig.jnglQ)
                {
                    ITEMS.CastHydra();
                    Spells.Q.Cast(mobs);
                }
                if (Spells.W.IsReady() && MenuConfig.jnglW)
                {
                    ITEMS.CastHydra();
                    Spells.W.Cast(mobs);
                }
            }
        }
        public static void Harass()
        {

        }
        public static void FastHarass()
        {

        }
    }
}
