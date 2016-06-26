using LeagueSharp.Common;
using LeagueSharp;
using SPrediction;
using System;
using SharpDX;

namespace Nechrito_Gragas
{
    class Mode
    {
        private static Obj_AI_Hero Player => ObjectManager.Player;

        public static Vector3 pred(Obj_AI_Hero Target)
        {
            var pos = Spells.R.GetVectorSPrediction(Target, 20).CastTargetPosition;

            if (Target != null && !pos.IsWall())
            {
                if (Target.IsFacing(Player))
                {
                    if (Target.IsMoving)
                    {
                        pos = pos.Extend(Player.Position.To2D(), - 70);
                    }
                    pos = pos.Extend(Player.Position.To2D(), - 70);
                }

                if (!Target.IsFacing(Player))
                {
                    if (Target.IsMoving)
                    {
                        pos = pos.Extend(Player.Position.To2D(), - 120);
                    }
                    pos = pos.Extend(Player.Position.To2D(), - 100);
                }
            }
            return pos.To3D2();
        }

        public static void ComboLogic()
        {
            var Target = TargetSelector.GetSelectedTarget();
           
            if (Target != null && Target.IsValidTarget() && !Target.IsZombie && (Program.Player.Distance(Target.Position) <= 900) && MenuConfig.ComboR)
            {
                if (Target.IsDashing()) return;
                if (Spells.Q.IsReady() && Spells.R.IsReady())
                {
                    
                    Spells.Q.Cast(pred(Target));
                    Spells.R.Cast(pred(Target));
                    Utility.DelayAction.Add(200, () => Spells.Q.Cast(Target));
                }
            }

             var target = TargetSelector.GetTarget(700f, TargetSelector.DamageType.Magical);

             if (target != null && target.IsValidTarget() && !target.IsZombie)
             {
                if (Spells.Q.IsReady())
                {
                    var pos = Spells.Q.GetSPrediction(target).CastPosition;
                    {
                        Spells.Q.Cast(pos);
                    }
                }

                // E
                if (Spells.E.IsReady() && !Spells.R.IsReady())
                {
                    var pos = Spells.E.GetPrediction(target).CastPosition;
                    {
                        Spells.E.Cast(pos);
                    }
                }

                // Smite
                if (Spells.Smite != SpellSlot.Unknown && Spells.R.IsReady() && Player.Spellbook.CanUseSpell(Spells.Smite) == SpellState.Ready && !Target.IsZombie)
                {
                    Player.Spellbook.CastSpell(Spells.Smite, Target);
                }

                else if (Spells.W.IsReady() && !Spells.E.IsReady())
                {
                    Spells.W.Cast();

                }
            }
        }
        
        public static void JungleLogic()
        {
            var mobs = MinionManager.GetMinions(Player.Position, Spells.W.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {   
                if (mobs.Count == 0 || mobs == null || Player.IsWindingUp)
                    return;
               
                foreach(var m in mobs)
                {
                    if(m.Distance(Player) <= 400f)
                    {
                        if (Spells.W.IsReady())
                        {
                            Spells.W.Cast();
                        }

                        if (Spells.E.IsReady())
                        {
                            Spells.E.Cast(m);
                        }

                        if (Spells.Q.IsReady())
                        {
                            Spells.Q.Cast(m);
                        }
                    }
                }
            }
        }
        public static void HarassLogic()
        {
            var target = TargetSelector.GetTarget(Spells.R.Range - 50, TargetSelector.DamageType.Magical);
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                if (Spells.E.IsReady() && MenuConfig.harassE)
                {
                    Spells.E.Cast(target);
                }
                    
                if (Spells.Q.IsReady() && MenuConfig.harassQ)
                {
                    var pos = Spells.Q.GetSPrediction(target).CastPosition;
                    Spells.Q.Cast(pos);
                }

                if(Spells.W.IsReady())
                {
                    if(target.Distance(Player) <= Player.AttackRange)
                    {
                        Spells.W.Cast();
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
    }
}
