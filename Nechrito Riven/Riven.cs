using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System.Collections.Generic;

namespace NechritoRiven
{
    public class Riven
    {
        public const string IsFirstR = "RivenFengShuiEngine";
        public const string IsSecondR = "RivenIzunaBlade";
        private const string MenuName = "Nechrito Riven";
        public static Menu Menu { get; set; } = new Menu(MenuName, MenuName, true);
        public static Obj_AI_Hero Player => ObjectManager.Player;

       
        public static int _qstack = 1;

        private static void OnDoCastLc(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || !Orbwalking.IsAutoAttack(args.SData.Name)) return;
            Logic._qtarget = (Obj_AI_Base)args.Target;

            if (args.Target is Obj_AI_Minion)
            {
                Modes.JungleLogic();
            }
        }

        private static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {

            var spellName = args.SData.Name;
            if (!sender.IsMe || !Orbwalking.IsAutoAttack(spellName)) return;
            if (args.Target is Obj_AI_Minion)
            {
                Modes.LaneLogic();
            }
            var @base = args.Target as Obj_AI_Turret;
            if (@base != null)
                if (@base.IsValid && args.Target != null && Spells._q.IsReady() && MenuConfig.LaneQ &&
                    MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear) Spells._q.Cast(@base);

            var hero = args.Target as Obj_AI_Hero;
            if (hero == null) return;
            var target = hero;

            if (Spells._r.IsReady() && Spells._r.Instance.Name == IsSecondR)
                if (target.Health < Dmg.Rdame(target, target.Health) + Player.GetAutoAttackDamage(target) &&
                    target.Health > Player.GetAutoAttackDamage(target)) Spells._r.Cast(target.Position);
            if (Spells._w.IsReady())
                if (target.Health < Spells._w.GetDamage(target) + Player.GetAutoAttackDamage(target) &&
                    target.Health > Player.GetAutoAttackDamage(target)) Spells._w.Cast();
            if (Spells._q.IsReady())
                if (target.Health < Spells._q.GetDamage(target) + Player.GetAutoAttackDamage(target) &&
                    target.Health > Player.GetAutoAttackDamage(target)) Spells._q.Cast(target);
        }
        private static void Interrupt(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (sender.IsEnemy && Spells._w.IsReady() && sender.IsValidTarget() && !sender.IsZombie)
            {
                if (sender.IsValidTarget(125 + Player.BoundingRadius + sender.BoundingRadius)) Spells._w.Cast();
            }
        }
        private static void OnTick(EventArgs args)
        {
            Killsteal();
            if (Utils.GameTimeTickCount - Logic._lastQ >= 3650 && _qstack >= 1 && !Player.IsRecalling() &&
                !Player.InFountain() && MenuConfig.KeepQ && Player.HasBuff("RivenTriCleave") &&
                !Player.Spellbook.IsChanneling &&
                Spells._q.IsReady()) Spells._q.Cast(Game.CursorPos);
            switch (MenuConfig._orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Modes.ComboLogic();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    Modes.JungleLogic();
                    Modes.LaneLogic();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Modes.HarassLogic();
                    break;
                case Orbwalking.OrbwalkingMode.FastHarass:
                    Modes.FastHarassLogic();
                    break;
                case Orbwalking.OrbwalkingMode.Burst:
                    Modes.BurstLogic();
                    break;
                case Orbwalking.OrbwalkingMode.Flee:
                    Modes.FleeLogic();
                    break;
            }
        }

        private static void Killsteal()
        {
            if (Spells._q.IsReady())
            {
                // R range because auto-gapclose! (Yes, i'm smart. Give Contrib pls)
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._r.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Spells._q.GetDamage(target) && Logic.InQRange(target))
                        Spells._q.Cast(target);
                }
            }
            if (Spells._r.IsReady() && Spells._r.Instance.Name == IsSecondR)
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._r.Range + Spells._e.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Spells._r.GetDamage(target) && !target.IsInvulnerable && (Player.Distance(target.Position) <= 1870) && (Player.Distance(target.Position) >= 1600))
                    {
                        Spells._e.Cast(target);
                        Utility.DelayAction.Add(90, () => Spells._r.Cast(target));
                    }

                }
            }
            if (Spells._w.IsReady())
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._r.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Spells._w.GetDamage(target) && Logic.InWRange(target))
                        Spells._w.Cast();
                }
            }
            if (Spells._r.IsReady() && Spells._r.Instance.Name == IsSecondR)
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._r.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Dmg.Rdame(target, target.Health) && !target.IsInvulnerable)
                        Spells._r.Cast(target.Position);
                }
            }
            if (Spells.Ignite.IsReady() && MenuConfig.ignite)
            {
                var target = TargetSelector.GetTarget(600f, TargetSelector.DamageType.True);
                if (target.IsValidTarget(600f) && Dmg.IgniteDamage(target) >= target.Health && !Spells._q.IsReady())
                {
                    Player.Spellbook.CastSpell(Spells.Ignite, target);
                }
            }
        }
        
        private static void OnPlay(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe) return;

            switch (args.Animation)
            {
                case "Spell1a":
                    Logic._lastQ = Utils.GameTimeTickCount;
                    if (MenuConfig.Qstrange && MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None)
                    {
                        if (MenuConfig.AnimDance) Game.Say("/d");
                        if (MenuConfig.AnimLaugh) Game.Say("/l");
                        if (MenuConfig.AnimTaunt) Game.Say("/t");
                        if (MenuConfig.AnimTalk) Game.Say("/j");
                    }
                    _qstack = 2;
                    if (MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None &&
                        MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LastHit &&
                        MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Flee)
                        Utility.DelayAction.Add(MenuConfig.Qd * 10 + 1, Reset);
                    break;
                case "Spell1b":
                    Logic._lastQ = Utils.GameTimeTickCount;
                    if (MenuConfig.Qstrange && MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None)
                    {
                        if (MenuConfig.AnimDance) Game.Say("/d");
                        if (MenuConfig.AnimLaugh) Game.Say("/l");
                        if (MenuConfig.AnimTaunt) Game.Say("/t");
                        if (MenuConfig.AnimTalk) Game.Say("/j");
                    }
                    _qstack = 3;
                    if (MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None &&
                        MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LastHit &&
                        MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Flee)
                        Utility.DelayAction.Add(MenuConfig.Qd * 10 + 1, Reset);
                    break;
                case "Spell1c":
                    Logic._lastQ = Utils.GameTimeTickCount;
                    if (MenuConfig.Qstrange && MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None)
                    {
                        if (MenuConfig.AnimDance) Game.Say("/d");
                        if (MenuConfig.AnimLaugh) Game.Say("/l");
                        if (MenuConfig.AnimTaunt) Game.Say("/t");
                        if (MenuConfig.AnimTalk) Game.Say("/j");
                    }
                    _qstack = 1;
                    if (MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None &&
                        MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LastHit &&
                        MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Flee)
                        Utility.DelayAction.Add(MenuConfig.Qld * 10 + 3, Reset);
                    break;
                case "Spell3":
                    if ((MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Burst ||
                         MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo ||
                         MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.FastHarass ||
                         MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Flee)) Logic.CastYoumoo();
                    break;
                case "Spell4a":
                    Logic._lastR = Utils.GameTimeTickCount;
                    break;
                case "Spell4b":
                    var target = TargetSelector.GetSelectedTarget();
                    if (Spells._q.IsReady() && target.IsValidTarget()) Logic.ForceCastQ(target);
                    break;
            }
        }

        private static void Reset()
        {
            if (MenuConfig.QReset) Game.Say("/d");
            Orbwalking.LastAATick = 0;
            Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position.Extend(Game.CursorPos, Player.Distance(Game.CursorPos) + 10));
            Player.IssueOrder(GameObjectOrder.MoveTo,
                 Player.Position - 115);
        }
        private static void OnCasting(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsEnemy && sender.Type == Player.Type)
            {
                var epos = Player.ServerPosition +
                           (Player.ServerPosition - sender.ServerPosition).Normalized() * 300;

                if (Player.Distance(sender.ServerPosition) <= args.SData.CastRange)
                {
                    switch (args.SData.TargettingType)
                    {
                        case SpellDataTargetType.Unit:

                            if (args.Target.NetworkId == Player.NetworkId)
                            {
                                if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit || MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear &&
                                    !args.SData.Name.Contains("NasusW"))
                                {
                                    if (Spells._e.IsReady()) Spells._e.Cast(epos);
                                }
                            }
                            break;
                        case SpellDataTargetType.SelfAoe:

                            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit || MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
                            {
                                if (Spells._e.IsReady()) Spells._e.Cast(epos);
                            }
                            break;
                    }
                    if (args.SData.Name.Contains("IreliaEquilibriumStrike"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._w.IsReady() && Logic.InWRange(sender)) Spells._w.Cast();
                            else if (Spells._e.IsReady()) Spells._e.Cast(epos);
                        }
                    }
                    if (args.SData.Name.Contains("TalonCutthroat"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._w.IsReady()) Spells._w.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("RenektonPreExecute"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._w.IsReady()) Spells._w.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("GarenRPreCast"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast(epos);
                        }
                    }

                    if (args.SData.Name.Contains("GarenQAttack"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast(Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position.Extend(Game.CursorPos, Player.Distance(Game.CursorPos) + 10)));
                        }
                    }

                    if (args.SData.Name.Contains("XenZhaoThrust3"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._w.IsReady()) Spells._w.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("RengarQ"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("RengarPassiveBuffDash"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("RengarPassiveBuffDashAADummy"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("TwitchEParticle"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("FizzPiercingStrike"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast(Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position.Extend(Game.CursorPos, Player.Distance(Game.CursorPos) + 10)));
                        }
                    }
                    if (args.SData.Name.Contains("HungeringStrike"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("YasuoDash"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("KatarinaRTrigger"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._w.IsReady() && Logic.InWRange(sender)) Spells._w.Cast();
                            else if (Spells._e.IsReady()) Spells._e.Cast(Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position.Extend(Game.CursorPos, Player.Distance(Game.CursorPos) + 10)));
                        }
                    }
                    if (args.SData.Name.Contains("KatarinaE"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._w.IsReady()) Spells._w.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("MonkeyKingQAttack"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("MonkeyKingSpinToWin"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast(Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position.Extend(Game.CursorPos, Player.Distance(Game.CursorPos) + 10)));
                            else if (Spells._w.IsReady()) Spells._w.Cast();
                        }
                    }
                }
            }
        }
    }
}