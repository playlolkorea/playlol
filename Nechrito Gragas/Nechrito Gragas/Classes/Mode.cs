using LeagueSharp.Common;
using LeagueSharp;
using SharpDX;
using SPrediction;
using System;

namespace Nechrito_Gragas
{
    class Mode
    {
        private static Obj_AI_Hero Player => ObjectManager.Player;
        public static void ComboLogic()
        {
            var Target = TargetSelector.GetSelectedTarget();
            if (Target != null && Target.IsValidTarget() && !Target.IsZombie && (Program.Player.Distance(Target.Position) <= 900) && MenuConfig.ComboR)
            {
                if (Spells._q.IsReady() && Spells._r.IsReady() && !Target.IsDashing())
                {
                    var pos = Spells._r.GetSPrediction(Target).CastPosition + 110;
                    {
                        Spells._q.SPredictionCast(Target, HitChance.VeryHigh);
                        Spells._r.Cast(pos);
                    }
                }
                var target = TargetSelector.GetTarget(700f, TargetSelector.DamageType.Magical);
                if (target != null && target.IsValidTarget() && !target.IsZombie)
                {
                    if (Spells._q.IsReady() && !Spells._r.IsReady())
                    {
                        var pos = Spells._q.GetSPrediction(target).CastPosition;
                        Spells._q.Cast(pos);
                    }
                    else if (Spells._r.IsReady() && !MenuConfig.OnlyR)
                    { 
                        var pos = Spells._r.GetSPrediction(target).CastPosition + 100;
                        if(Target.IsFacing(Program.Player))
                        {
                            Spells._r.Cast(pos);
                        }
                        if(!Target.IsFacing(Program.Player))
                        {
                            Spells._r.Cast(pos + 35);
                        }
                    }
                    // E
                    else if (Spells._e.IsReady())
                    {
                        var pos = Spells._e.GetPrediction(target).CastPosition;
                        Spells._e.Cast(pos);
                    }
                    // Smite
                    else if (Spells.Smite != SpellSlot.Unknown && Spells._r.IsReady()
                          && Player.Spellbook.CanUseSpell(Spells.Smite) == SpellState.Ready && !Target.IsZombie)
                        Player.Spellbook.CastSpell(Spells.Smite, Target);
                    // W
                    else if (Spells._w.IsReady() && !Spells._e.IsReady())
                        Spells._w.Cast();
                }
            }
        }
        
        public static void JungleLogic()
        {
            var mobs = MinionManager.GetMinions(400 + Program.Player.AttackRange, MinionTypes.All, MinionTeam.Neutral,
           MinionOrderTypes.MaxHealth);
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {   
                if (mobs.Count == 0 || mobs == null)
                    return;

                if (Spells._w.IsReady())
                    Spells._w.Cast();
                if (Spells._e.IsReady())
                    Spells._e.Cast(mobs[0]);
                if (Spells._q.IsReady())
                    Spells._q.Cast(mobs[0]);
            }
        }
        public static void HarassLogic()
        {
            var target = TargetSelector.GetTarget(Spells._r.Range - 50, TargetSelector.DamageType.Magical);
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                if (Spells._e.IsReady())
                    Spells._e.Cast(target);
                if (Spells._q.IsReady())
                {
                    var pos = Spells._q.GetSPrediction(target).CastPosition;
                    Spells._q.Cast(pos);
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
