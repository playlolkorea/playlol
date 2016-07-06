using Dark_Star_Thresh.Core;
using LeagueSharp.Common;
using LeagueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using LeagueSharp.Common.Data;

namespace Dark_Star_Thresh.Update
{
    class Mode : Core.Core
    {
        public static Vector3 qPred(Obj_AI_Hero Target)
        {
            var pos = Spells.Q.GetPrediction(Target).CastPosition.To2D();


            return pos.To3D2();
        }

        public static void GetActiveMode(EventArgs args)
        {
            switch (_orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.None:
                    FlashCombo();
                    break;
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Harass();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    break;
                case Orbwalking.OrbwalkingMode.LastHit:
                    LastHit();
                    break;
            }
        }

        public static void Combo()
        {
            var qTarget = TargetSelector.GetTarget(Spells.Q.Range, TargetSelector.DamageType.Physical);

            var eTarget = TargetSelector.GetTarget(Spells.E.Range, TargetSelector.DamageType.Physical);

            var rTarget = TargetSelector.GetTarget(Spells.R.Range, TargetSelector.DamageType.Physical);

            // Credits to DanZ for this line of code.
            var wAlly = Player.GetAlliesInRange(Spells.W.Range).Where(x => !x.IsMe).Where(x => !x.IsDead).Where(x => x.Distance(Player.Position) <= Spells.W.Range + 250).FirstOrDefault();

            if (Spells.E.IsReady())
            {
                if (eTarget != null && !eTarget.IsDashing() && !eTarget.IsDead && eTarget.IsValidTarget(Spells.E.Range))
                {
                    if(eTarget.CountAlliesInRange(1400) == 0)
                    {
                        Spells.E.Cast(eTarget.Position);
                    }
                    else
                    {
                        if (eTarget.Distance(Player.Position) <= Spells.E.Range)
                        {
                            Spells.E.Cast(Player.Position.Extend(eTarget.Position, Vector3.Distance(eTarget.Position, Player.Position) + 400));
                        }
                    }
                }
            }

            if(Spells.Q.IsReady())
            {
                if (qTarget != null && !qTarget.IsDashing() && !qTarget.IsDead && qTarget.IsValidTarget())
                {
                    var qPrediction = Spells.Q.GetPrediction(qTarget);

                    if (Spells.Q.WillHit(qTarget, qPrediction.CastPosition))
                    {
                        Spells.Q.Cast(qPred(qTarget));
                    }
                }
            }

            if (wAlly != null)
            {
                if (Spells.W.IsReady())
                {
                    Spells.W.Cast(wAlly);
                }
            }

            if (rTarget != null && !rTarget.IsDead && rTarget.IsValidTarget())
            {
                if (MenuConfig.ComboR >= rTarget.CountEnemiesInRange(Spells.R.Range - 30))
                {
                    if (Spells.R.IsReady())
                    {
                        Spells.R.Cast();
                    }
                }
            }
        }

        public static void Harass()
        {
            var qTarget = TargetSelector.GetTarget(Spells.Q.Range, TargetSelector.DamageType.Physical);

            var eTarget = TargetSelector.GetTarget(Spells.E.Range, TargetSelector.DamageType.Physical);

            if(MenuConfig.HarassE)
            {
                if (Spells.E.IsReady())
                {
                    if (eTarget != null && !eTarget.IsDashing() && !eTarget.IsDead && eTarget.IsValidTarget())
                    {
                        if (eTarget.Distance(Player.Position) <= Spells.E.Range)
                        {
                            Spells.E.Cast(eTarget.Position);
                        }
                    }
                }
            }

            // The reason we can return here is because we wont go further. Better code and we don't have to put unecessary if statements
            if (!MenuConfig.HarassQ) return;
            
            if (qTarget == null || qTarget.IsDashing() || qTarget.IsDead || !qTarget.IsValidTarget() || !Spells.Q.IsReady()) return;

                Spells.Q.Cast(qPred(qTarget));
        }

        public static void LastHit()
        {
            var minions = MinionManager.GetMinions(Player.AttackRange);

            foreach(var m in minions)
            {
                if(m.IsValidTarget(1400) && m != null && !m.IsDead)
                {
                    var range = Player.GetAlliesInRange(Spells.W.Range).Where(x => !x.IsMe).Where(x => !x.IsDead).Where(x => x.Distance(Player.Position) <= 1400).FirstOrDefault();

                    if (Dmg.StackDmg(m) >= m.Health && range != null)
                    {
                        Render.Circle.DrawCircle(m.Position, 75, System.Drawing.Color.MediumVioletRed);
                    }
                }
            }
        }

        public static void FlashCombo()
        {
            if (!MenuConfig.ComboFlash) return;

            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

            if (!Spells.Q.IsReady()) return;

            var qTarget = TargetSelector.GetTarget(Spells.Q.Range + 300, TargetSelector.DamageType.Physical);

            if(qTarget != null && qTarget.IsValidTarget())
            {
                var qPrediction = Spells.Q.GetPrediction(qTarget);
                if(qPrediction.Hitchance >= HitChance.Collision)
                {
                    var wAlly = Player.GetAlliesInRange(Spells.W.Range).Where(x => !x.IsMe).Where(x => !x.IsDead).Where(x => x.Distance(Player.Position) <= Spells.W.Range + 250).FirstOrDefault();
                    if (wAlly != null)
                    {
                        Spells.W.Cast(wAlly);
                    }

                    if (Spells.Flash != SpellSlot.Unknown && Player.Spellbook.CanUseSpell(Spells.Flash) == SpellState.Ready)
                    {
                        Player.Spellbook.CastSpell(Spells.Flash, qPrediction.CastPosition);
                        Spells.Q.Cast(qPrediction.CastPosition);
                    }
                }    
            }   
        }
    }
}
