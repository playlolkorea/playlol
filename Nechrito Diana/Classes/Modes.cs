using LeagueSharp;
using LeagueSharp.Common;
using SPrediction;
using System;
using System.Linq;

namespace Nechrito_Diana
{
    class Modes
    {
        public static void ComboLogic()
        {
            var target = TargetSelector.GetTarget(850, TargetSelector.DamageType.Magical);
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                if((Program.Player.Distance(target.Position) <= 800f) && (Program.Player.Distance(target.Position) >= 680f) && Spells._q.IsReady() && Spells._r.IsReady())
                {
                    var t = TargetSelector.GetTarget(Spells._q.Range, TargetSelector.DamageType.Magical);
                    if(t != null)
                    {
                        Spells._r.SPredictionCast(t, HitChance.High);
                        Spells._q.SPredictionCast(t, HitChance.High);
                    }
                }
                else if (Spells._q.IsReady() && (Program.Player.Distance(target.Position) <= 700f))
                {
                    var t = TargetSelector.GetTarget(Spells._q.Range, TargetSelector.DamageType.Magical);
                    if (t != null)
                    {
                        Spells._q.SPredictionCast(t, HitChance.High);
                    }
                }
                else if (Spells._r.IsReady() && (Program.Player.Distance(target.Position) <= 700f))
                {
                    var t = TargetSelector.GetTarget(Spells._r.Range, TargetSelector.DamageType.Magical);
                    if (t != null)
                    {
                        Spells._r.SPredictionCast(t, HitChance.High);
                    }
                }
                else if (Spells._w.IsReady() && (Program.Player.Distance(target.Position) <= Program.Player.AttackRange))
                        Spells._w.Cast(target);
                else if (Spells._e.IsReady() && (Program.Player.Distance(target.Position) <= Spells._e.Range && (Program.Player.Distance(target.Position) >= 100)))
                        Spells._e.Cast(target);  
            }
        }
        public static void HarassLogic()
        {
            var target = TargetSelector.GetTarget(750, TargetSelector.DamageType.Magical);
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                if (Spells._q.IsReady() && (Program.Player.Distance(target.Position) <= 700f))
                {
                    var t = TargetSelector.GetTarget(Spells._q.Range, TargetSelector.DamageType.Magical);
                    if (t != null)
                    {
                        Spells._q.SPredictionCast(target, HitChance.High);
                    }
                }
                if (Spells._w.IsReady())
                    Spells._w.Cast(target);
            }
        }
        public static void JungleLogic()
        {
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                var mobs = MinionManager.GetMinions(800 + Program.Player.AttackRange, MinionTypes.All, MinionTeam.Neutral,
           MinionOrderTypes.MaxHealth);
                if (mobs.Count == 0)
                    return;

                if (Spells._q.IsReady() && Spells._r.IsReady())
                {
                    var m = MinionManager.GetMinions(800 + Program.Player.AttackRange, MinionTypes.All, MinionTeam.Neutral,
          MinionOrderTypes.MaxHealth);
                    if (m != null && MenuConfig.jnglQR && (Program.Player.Distance(m[0].Position) <= 700f) && (Program.Player.Distance(m[0].Position) >= 400f))
                    {
                        Spells._q.Cast(m[0]);
                        Spells._r.Cast(m[0]);
                    }
                }
                 if (Spells._w.IsReady() && (Program.Player.Distance(mobs[0].Position) <= 300f) && MenuConfig.jnglW)
                    Spells._w.Cast(mobs[0].ServerPosition);

                if (Spells._e.IsReady())
                {
                    var minion = MinionManager.GetMinions(Program.Player.Position, Spells._e.Range);
                    foreach (var m in mobs)
                    {
                        if (m.IsAttackingPlayer && Program.Player.HealthPercent <= MenuConfig.jnglE.MinValue)
                            Spells._e.Cast(m);
                    }
                }
            }
        }
        public static void LaneLogic()
        {
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                var minions = MinionManager.GetMinions(600f).FirstOrDefault();
                if (minions == null)
                    return;

                if (Spells._w.IsReady() && MenuConfig.LaneW && (Program.Player.Distance(minions.Position) <= 250f))
                {
                    Spells._w.Cast();
                }

                if (Spells._q.IsReady() && MenuConfig.LaneQ && Program.Player.ManaPercent <= 45 && (Program.Player.Distance(minions.Position) <= 550f))
                {
                    var minion = MinionManager.GetMinions(Program.Player.Position, Spells._w.Range);
                    foreach (var m in minion)
                    {
                        if (m.Health < Spells._q.GetDamage(m) && minion.Count > 2)
                            Spells._q.Cast(m);
                    }
                }
            }
        }
        public static void Game_OnUpdate(EventArgs args)
        {
            if (MenuConfig.UseSkin)
            {
                Program.Player.SetSkin(Program.Player.CharData.BaseSkinName, MenuConfig.Config.Item("Skin").GetValue<StringList>().SelectedIndex);
            }
            else Program.Player.SetSkin(Program.Player.CharData.BaseSkinName, Program.Player.BaseSkinId);
        }
        public static void Flee() // Sends Error
        {
            if(MenuConfig.FleeMouse)
                try
            {
                Program.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

                    var mob = MinionManager.GetMinions(800, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
                    var m = MinionManager.GetMinions(800, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.MaxHealth);
                    if (m[0].Distance(Game.CursorPos) <= 750 && m[0].Distance(Program.Player) >= 650 && m != null)
                    {
                    if(Spells._q.IsReady() && Spells._r.IsReady())
                        {
                            Spells._q.Cast(m[0].ServerPosition);
                            Spells._r.Cast(m[0].ServerPosition);
                        }
                    if(Spells._r.IsReady())
                        {
                            Spells._r.Cast(m[0].ServerPosition);
                        }
                    }
                    if (mob[0].Distance(Game.CursorPos) <= 750 && mob[0].Distance(Program.Player) >= 650 && mob != null)
                    {
                        if (Spells._q.IsReady() && Spells._r.IsReady())
                        {
                            Spells._q.Cast(mob[0].ServerPosition);
                            Spells._r.Cast(mob[0].ServerPosition);
                        }
                        if (Spells._r.IsReady())
                        {
                            Spells._r.Cast(mob[0].ServerPosition);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
        }
        }
    }
