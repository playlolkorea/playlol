using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using ItemData = LeagueSharp.Common.Data.ItemData;

namespace NechritoRiven
{
    public class Program
    {
        private const string IsFirstR = "RivenFengShuiEngine";
        private const string IsSecondR = "rivenizunablade";
        public static Menu Menu;
        private static Orbwalking.Orbwalker _orbwalker;
        private static readonly Obj_AI_Hero Player = ObjectManager.Player;
        private static readonly HpBarIndicator Indicator = new HpBarIndicator();
        public static SpellSlot Ignite, Flash;
        private static Spell _q, _q2, _q3, _w, _e, _r;
        private static int _qStack = 1;
        public static Render.Text Timer, Timer2;
        private static bool _forceQ;
        private static bool _forceW;
        private static bool _forceR;
        private static bool _forceR2;
        private static bool _forceItem;
        private static float _lastQ;
        private static float _lastR;
        private static AttackableUnit _qTarget;
        private static bool DoIgnite => Menu.Item("doIgnite").GetValue<bool>();
        private static bool Dind => Menu.Item("Dind").GetValue<bool>();
        private static bool DrawCb => Menu.Item("DrawCB").GetValue<bool>();
        private static bool StunBurst => Menu.Item("StunBurst").GetValue<bool>();
        private static bool KillstealW => Menu.Item("killstealw").GetValue<bool>();

        private static bool KillstealQ => Menu.Item("killstealq").GetValue<bool>();

        private static bool KillstealR => Menu.Item("killstealr").GetValue<bool>();

        private static bool DrawAlwaysR => Menu.Item("DrawAlwaysR").GetValue<bool>();
        private static bool DrawLogic => Menu.Item("DrawLogic").GetValue<bool>();

        private static bool DrawFh => Menu.Item("DrawFH").GetValue<bool>();
        private static bool DrawTimer1 => Menu.Item("DrawTimer1").GetValue<bool>();
        private static bool DrawTimer2 => Menu.Item("DrawTimer2").GetValue<bool>();
        private static bool DrawHs => Menu.Item("DrawHS").GetValue<bool>();
        private static bool DrawBt => Menu.Item("DrawBT").GetValue<bool>();

        private static bool UseLogic => Menu.Item("UseLogic").GetValue<KeyBind>().Active;
        private static bool AlwaysR => Menu.Item("AlwaysR").GetValue<KeyBind>().Active;
        private static bool AutoShield => Menu.Item("AutoShield").GetValue<bool>();
        private static int Qd => Menu.Item("QD").GetValue<Slider>().Value;
        private static int Qld => Menu.Item("QLD").GetValue<Slider>().Value;
        private static int AutoW => Menu.Item("AutoW").GetValue<Slider>().Value;
        private static bool ComboW => Menu.Item("ComboW").GetValue<bool>();

        private static bool RMaxDam => Menu.Item("RMaxDam").GetValue<bool>();
        private static bool RKillable => Menu.Item("RKillable").GetValue<bool>();
        private static bool LaneW => Menu.Item("LaneW").GetValue<bool>();
        private static bool LaneE => Menu.Item("LaneE").GetValue<bool>();
        private static bool WInterrupt => Menu.Item("WInterrupt").GetValue<bool>();
        private static bool Qstrange => Menu.Item("Qstrange").GetValue<bool>();
        private static bool LaneQ => Menu.Item("LaneQ").GetValue<bool>();
        private static bool Youmu => Menu.Item("youmu").GetValue<bool>();

        public static bool IsLethal(Obj_AI_Base unit)
        {
            return Totaldame(unit) / 1.65 >= unit.Health;
        }

        private static Obj_AI_Base GetCenterMinion()
        {
            var minionposition = MinionManager.GetMinions(300 + _q.Range).Select(x => x.Position.To2D()).ToList();
            var center = MinionManager.GetBestCircularFarmLocation(minionposition, 250, 300 + _q.Range);

            return center.MinionsHit >= 3
                ? MinionManager.GetMinions(1000).OrderBy(x => x.Distance(center.Position)).FirstOrDefault()
                : null;
        }

        private static int Item
            =>
                Items.CanUseItem(3077) && Items.HasItem(3077)
                    ? 3077
                    : Items.CanUseItem(3074) && Items.HasItem(3074) ? 3074 : 0;

        private static int GetWRange => Player.HasBuff("RivenFengShuiEngine") ? 330 : 265;


        private static void Main() => CustomEvents.Game.OnGameLoad += OnGameLoad;

        private static void OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Riven") return;
            Game.PrintChat("<b><font color=\"#00e5e5\">Nechrito Riven, GL & HF</font></b>");
            _q = new Spell(SpellSlot.Q, 260f);
            _q.SetSkillshot(0.25f, 100f, 2200f, false, SkillshotType.SkillshotCircle);
            _w = new Spell(SpellSlot.W, 250f);
            _e = new Spell(SpellSlot.E, 270);
            _r = new Spell(SpellSlot.R, 900);
            _r.SetSkillshot(0.25f, (float)(45 * 0.5), 1600, false, SkillshotType.SkillshotCircle);
            Ignite = Player.GetSpellSlot("SummonerDot");
            Flash = Player.GetSpellSlot("SummonerFlash");

            OnMenuLoad();
            Timer =
                new Render.Text(
                    "Q Expiry =>  " + ((double)(_lastQ - Utils.GameTimeTickCount + 3800) / 1000).ToString("0.0"),
                    (int)Drawing.WorldToScreen(Player.Position).X - 140,
                    (int)Drawing.WorldToScreen(Player.Position).Y + 10, 30, Color.DodgerBlue, "calibri");
            Timer2 =
                new Render.Text(
                    "R Expiry =>  " + (((double)_lastR - Utils.GameTimeTickCount + 15000) / 1000).ToString("0.0"),
                    (int)Drawing.WorldToScreen(Player.Position).X - 60,
                    (int)Drawing.WorldToScreen(Player.Position).Y + 10, 30, Color.DodgerBlue, "calibri");

            Game.OnUpdate += OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnEndScene += Drawing_OnEndScene;
            Obj_AI_Base.OnProcessSpellCast += OnCast;
            Obj_AI_Base.OnDoCast += OnDoCast;
            Obj_AI_Base.OnDoCast += OnDoCastLc;
            Obj_AI_Base.OnPlayAnimation += OnPlay;
            Obj_AI_Base.OnProcessSpellCast += OnCasting;
            Interrupter2.OnInterruptableTarget += Interrupt;
        }

        private static bool HasTitan() => Items.HasItem(3748) && Items.CanUseItem(3748);

        private static void CastTitan()
        {
            if (Items.HasItem(3748) && Items.CanUseItem(3748))
            {
                Items.UseItem(3748);
                Orbwalking.LastAATick = 0;
            }
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (
                var enemy in
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(ene => ene.IsValidTarget() && !ene.IsZombie))
            {
                if (Dind)
                {
                    Indicator.unit = enemy;
                    Indicator.drawDmg(GetComboDamage(enemy), new ColorBGRA(255, 204, 0, 170));
                }
            }
        }

        private static void OnDoCastLc(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || !Orbwalking.IsAutoAttack(args.SData.Name)) return;
            _qTarget = (Obj_AI_Base)args.Target;
            if (args.Target is Obj_AI_Minion)
            {
                if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
                {
                    var minions = MinionManager.GetMinions(70 + 120 + Player.BoundingRadius);
                    if (minions.Count >= 1)
                    {
                        if (HasTitan())
                        {
                            CastTitan();
                            return;
                        }
                        if (_e.IsReady() && _e.IsReady() &&
                            !minions[0].UnderTurret() && LaneE)

                            _e.Cast(GetCenterMinion().IsValidTarget() ? GetCenterMinion() : minions[0]);


                        if (_q.IsReady() && (Player.Distance(Player.ServerPosition) <= 250 + Player.AttackRange) &&
                            LaneQ)

                            _q.Cast(GetCenterMinion().IsValidTarget() ? GetCenterMinion() : minions[1]);


                        if (_w.IsReady() && minions.Count >= 2 && LaneW)
                            _w.Cast(GetCenterMinion());
                    }
                }
            }
        }

        private static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var spellName = args.SData.Name;
            if (!sender.IsMe || !Orbwalking.IsAutoAttack(spellName)) return;
            _qTarget = (Obj_AI_Base)args.Target;

            if (args.Target is Obj_AI_Minion)
            {
                if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
                {
                    var mobs = MinionManager.GetMinions(70 + 120 + Player.BoundingRadius, MinionTypes.All,
                        MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
                    if (mobs.Count != 0)
                    {
                        if (_e.IsReady() && !Orbwalking.InAutoAttackRange(mobs[0]))
                        {
                            _e.Cast(mobs[0].Position);
                        }

                        if (HasTitan())
                        {
                            CastTitan();
                            return;
                        }
                        if (_q.IsReady())
                        {
                            ForceItem();
                            Utility.DelayAction.Add(1, () => ForceCastQ(mobs[0]));
                        }
                        if (_w.IsReady())
                        {
                            ForceItem();
                            Utility.DelayAction.Add(1, ForceW);
                        }

                        else if (_e.IsReady())
                        {
                            _e.Cast(mobs[0].Position);
                        }
                    }
                }
            }
            var @base = args.Target as Obj_AI_Turret;
            if (@base != null)
                if (@base.IsValid && args.Target != null && _q.IsReady() && LaneQ &&
                    _orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear) ForceCastQ(@base);
            var hero = args.Target as Obj_AI_Hero;
            if (hero == null) return;
            var target = hero;
            if (KillstealR && _r.IsReady() && _r.Instance.Name == IsSecondR)
                if (target.Health < Rdame(target, target.Health) + Player.GetAutoAttackDamage(target) &&
                    target.Health > Player.GetAutoAttackDamage(target)) _r.Cast(target.Position);
            if (KillstealW && _w.IsReady())
                if (target.Health < _w.GetDamage(target) + Player.GetAutoAttackDamage(target) &&
                    target.Health > Player.GetAutoAttackDamage(target)) _w.Cast();
            if (KillstealQ && _q.IsReady())
                if (target.Health < _q.GetDamage(target) + Player.GetAutoAttackDamage(target) &&
                    target.Health > Player.GetAutoAttackDamage(target)) _q.Cast(target);
            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                if (HasTitan())
                {
                    CastTitan();
                    return;
                }
                if (_e.IsReady())
                    _e.Cast(target.Position);


                if (_w.IsReady() && InWRange(target))
                    _w.Cast();

                if (_q.IsReady())
                {
                    ForceItem();
                    Utility.DelayAction.Add(1, () => ForceCastQ(target));
                }

                if (_r.IsReady() && _qStack == 2 && _r.Instance.Name == IsSecondR)
                    _r.Cast(target.Position);
            }


            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
            {
                if (HasTitan())
                {
                    CastTitan();
                    return;
                }
                if (_qStack == 2 && _q.IsReady())
                {
                    ForceItem();
                    Utility.DelayAction.Add(1, () => ForceCastQ(target));
                }
            }

            if (_orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Burst) return;

            if (_e.IsReady())
                _e.Cast(target.Position);

            if (_w.IsReady() && InWRange(target))
                _w.Cast();

            if (_q.IsReady())
            {
                ForceItem();
                Utility.DelayAction.Add(1, () => ForceCastQ(target));
            }
            if (_r.IsReady() && _qStack == 2 && _r.Instance.Name == IsSecondR)
                _r.Cast(target.Position);

            if (_r.IsReady() && StunBurst && _r.Instance.Name == IsSecondR)
                _r.Cast(target.Position);






        }

        private static void OnMenuLoad()
        {
            Menu = new Menu("Nechrito Riven", "nechritoriven", true);
            var ts = Menu.AddSubMenu(new Menu("Target Selector", "Target Selector"));
            TargetSelector.AddToMenu(ts);
            var orbwalker = new Menu("Orbwalk", "rorb");
            _orbwalker = new Orbwalking.Orbwalker(orbwalker);
            Menu.AddSubMenu(orbwalker);

            var combo = new Menu("Combo", "Combo");

            combo.AddItem(new MenuItem("StunBurst", "Flash Q3 Burst").SetValue(false));
            combo.AddItem(new MenuItem("AlwaysR", "Use R").SetValue(new KeyBind('G', KeyBindType.Toggle)));
            combo.AddItem(new MenuItem("UseLogic", "Use Logic").SetValue(new KeyBind('L', KeyBindType.Toggle)));
            combo.AddItem(new MenuItem("doIgnite", "Use Ignite").SetValue(true));
            combo.AddItem(new MenuItem("ComboW", "Always use W").SetValue(true));
            combo.AddItem(new MenuItem("RKillable", "Smart R").SetValue(true));
            combo.AddItem(new MenuItem("killstealw", "Killsteal W").SetValue(true));
            combo.AddItem(new MenuItem("killstealq", "Killsteal Q").SetValue(true));
            combo.AddItem(new MenuItem("killstealr", "Killsteal Second R").SetValue(true));

            Menu.AddSubMenu(combo);
            var lane = new Menu("Lane", "Lane");
            lane.AddItem(new MenuItem("LaneQ", "Use Q").SetValue(true));
            lane.AddItem(new MenuItem("LaneW", "Use W").SetValue(true));
            lane.AddItem(new MenuItem("LaneE", "Use E").SetValue(true));



            Menu.AddSubMenu(lane);
            var misc = new Menu("Misc", "Misc");
            misc.AddItem(new MenuItem("youmu", "Auto Yomuu's").SetValue(true));
            misc.AddItem(new MenuItem("Qstrange", "Fast Q, not legit!").SetValue(false));
            misc.AddItem(new MenuItem("RMaxDam", "R2 Max Dmg").SetValue(true));
            misc.AddItem(new MenuItem("AutoShield", "Auto Cast E").SetValue(true));
            misc.AddItem(new MenuItem("AutoW", "Auto W When x Enemy").SetValue(new Slider(5, 0, 5)));
            misc.AddItem(new MenuItem("Winterrupt", "W interrupt").SetValue(true));
            misc.AddItem(new MenuItem("KeepQ", "Keep Q Alive").SetValue(true));
            misc.AddItem(new MenuItem("QD", "Q1, Q2 Delay").SetValue(new Slider(29, 23, 43)));
            misc.AddItem(new MenuItem("QLD", "Q3 Delay").SetValue(new Slider(39, 36, 53)));

            Menu.AddSubMenu(misc);

            var draw = new Menu("Draw", "Draw");
            draw.AddItem(new MenuItem("DrawTimer1", "Draw Q Expiry Time").SetValue(false));
            draw.AddItem(new MenuItem("DrawTimer2", "Draw R Expiry Time").SetValue(false));
            draw.AddItem(new MenuItem("DrawAlwaysR", "R Status").SetValue(true));
            draw.AddItem(new MenuItem("DrawLogic", "Logic").SetValue(true));
            draw.AddItem(new MenuItem("Dind", "Damage Indicator").SetValue(true));
            draw.AddItem(new MenuItem("DrawCB", "Combo Engage").SetValue(false));
            draw.AddItem(new MenuItem("DrawBT", "Burst Engage").SetValue(false));
            draw.AddItem(new MenuItem("DrawFH", "FastHarass Engage").SetValue(false));
            draw.AddItem(new MenuItem("DrawHS", "Harass Engage").SetValue(false));

            Menu.AddSubMenu(draw);

            var credit = new Menu("Credit", "Credit");

            credit.AddItem(new MenuItem("hoola", "Made by Hoola, Configured By Nechrito"));

            Menu.AddSubMenu(credit);

            Menu.AddToMainMenu();
        }

        private static void Interrupt(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (sender.IsEnemy && _w.IsReady() && sender.IsValidTarget() && !sender.IsZombie && WInterrupt)
            {
                if (sender.IsValidTarget(125 + Player.BoundingRadius + sender.BoundingRadius)) _w.Cast();
            }
        }

        private static void AutoUseW()
        {
            if (AutoW > 0)
            {
                if (Player.CountEnemiesInRange(GetWRange) >= AutoW)
                {
                    ForceW();
                }
            }
        }

        private static void OnTick(EventArgs args)
        {
            Timer.X = (int)Drawing.WorldToScreen(Player.Position).X - 60;
            Timer.Y = (int)Drawing.WorldToScreen(Player.Position).Y + 43;
            Timer2.X = (int)Drawing.WorldToScreen(Player.Position).X - 60;
            Timer2.Y = (int)Drawing.WorldToScreen(Player.Position).Y + 65;
            ForceSkill();
            UseRMaxDam();
            AutoUseW();
            Killsteal();
            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo) Combo();
            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear) Jungleclear();
            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed) Harass();
            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.FastHarass) FastHarass();
            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Burst) Burst();
            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Flee) Flee();
            if (Utils.GameTimeTickCount - _lastQ >= 3650 && _qStack != 1 && !Player.IsRecalling() &&
                !Player.InFountain() &&
                !Player.Spellbook.IsChanneling &&
                _q.IsReady()) _q.Cast(Game.CursorPos);
        }

        private static void Killsteal()
        {
            if (KillstealQ && _q.IsReady())
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(_r.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < _q.GetDamage(target) && InQRange(target))
                        _q.Cast(target);
                }
            }
            if (KillstealW && _w.IsReady())
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(_r.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < _w.GetDamage(target) && InWRange(target))
                        _w.Cast();
                }
            }
            if (KillstealR && _r.IsReady() && _r.Instance.Name == IsSecondR)
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(_r.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Rdame(target, target.Health) && !target.HasBuff("kindrednodeathbuff") &&
                        !target.HasBuff("Undying Rage") && !target.HasBuff("JudicatorIntervention"))
                        _r.Cast(target.Position);
                }
            }
        }

        private static void UseRMaxDam()
        {
            if (RMaxDam && _r.IsReady() && _r.Instance.Name == IsSecondR)
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(_r.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health / target.MaxHealth <= 0.25 &&
                        (!target.HasBuff("kindrednodeathbuff") || !target.HasBuff("Undying Rage") ||
                         !target.HasBuff("JudicatorIntervention")))
                        _r.Cast(target.Position);
                }
            }
            else if (DoIgnite)
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(_r.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health / target.MaxHealth <= 0.25 &&
                        (!target.HasBuff("kindrednodeathbuff") || !target.HasBuff("Undying Rage") ||
                         !target.HasBuff("JudicatorIntervention")))
                        Player.Spellbook.CastSpell(Ignite, target);
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead)
                return;
            var heropos = Drawing.WorldToScreen(ObjectManager.Player.Position);


            if (_qStack != 1 && DrawTimer1)
            {
                Timer.text = "Q Expiry =>  " + ((double)(_lastQ - Utils.GameTimeTickCount + 3800) / 1000).ToString("0.0") +
                             "S";
                Timer.OnEndScene();
            }

            if (Player.HasBuff("RivenFengShuiEngine") && DrawTimer2)
            {
                Timer2.text = "R Expiry =>  " +
                              (((double)_lastR - Utils.GameTimeTickCount + 15000) / 1000).ToString("0.0") + "S";
                Timer2.OnEndScene();
            }

            if (DrawCb)
                Render.Circle.DrawCircle(Player.Position, 250 + Player.AttackRange + 70,
                    _e.IsReady() ? System.Drawing.Color.FromArgb(120, 0, 170, 255) : System.Drawing.Color.IndianRed);
            if (DrawBt && Flash != SpellSlot.Unknown)
                Render.Circle.DrawCircle(Player.Position, 750,
                    _r.IsReady() && Flash.IsReady()
                        ? System.Drawing.Color.FromArgb(120, 0, 170, 255)
                        : System.Drawing.Color.IndianRed);

            if (DrawFh)
                Render.Circle.DrawCircle(Player.Position, 450 + Player.AttackRange + 70,
                    _e.IsReady() && _q.IsReady()
                        ? System.Drawing.Color.FromArgb(120, 0, 170, 255)
                        : System.Drawing.Color.IndianRed);
            if (DrawHs)
                Render.Circle.DrawCircle(Player.Position, 400,
                    _q.IsReady() && _w.IsReady()
                        ? System.Drawing.Color.FromArgb(120, 0, 170, 255)
                        : System.Drawing.Color.IndianRed);
            if (DrawAlwaysR)
            {
                Drawing.DrawText(heropos.X - 15, heropos.Y + 20, System.Drawing.Color.DodgerBlue, "Use R  (     )");
                Drawing.DrawText(heropos.X + 40, heropos.Y + 20,
                    AlwaysR ? System.Drawing.Color.LimeGreen : System.Drawing.Color.Red, AlwaysR ? "On" : "Off");
            }

            if (DrawLogic)
            {
                Drawing.DrawText(heropos.X - 15, heropos.Y + 36, System.Drawing.Color.DodgerBlue, "Logic  (     )");
                Drawing.DrawText(heropos.X + 37, heropos.Y + 36,
                    UseLogic ? System.Drawing.Color.LimeGreen : System.Drawing.Color.Red, UseLogic ? "On" : "Off");
            }
        }

        private static void Jungleclear()
        {
            var mobs = MinionManager.GetMinions(190 + Player.AttackRange + 70, MinionTypes.All, MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth);

            if (mobs.Count <= 0)
                return;

            if (_e.IsReady() && !Orbwalking.InAutoAttackRange(mobs[0]))
            {
                _e.Cast(mobs[0].Position);
            }
        }

        private static void Combo()
        {
            var targetR = TargetSelector.GetTarget(250 + Player.AttackRange + 70, TargetSelector.DamageType.Physical);
            if (DoIgnite)
            {
                if (targetR.HealthPercent < 25 && Ignite.IsReady())
                    Player.Spellbook.CastSpell(Ignite, targetR);
            }
            if (_r.IsReady() && _r.Instance.Name == IsFirstR && _orbwalker.InAutoAttackRange(targetR) && AlwaysR &&
                targetR != null) ForceR();
            if (_r.IsReady() && _r.Instance.Name == IsFirstR && _w.IsReady() && InWRange(targetR) && ComboW && AlwaysR &&
                targetR != null)
            {
                ForceR();
                Utility.DelayAction.Add(1, ForceW);
            }
            if (_w.IsReady() && InWRange(targetR) && ComboW && targetR != null) _w.Cast();
            if (UseLogic && _r.IsReady() && _r.Instance.Name == IsFirstR && _w.IsReady() && targetR != null &&
                _e.IsReady() &&
                targetR.IsValidTarget() && !targetR.IsZombie && (IsKillableR(targetR) || AlwaysR))
            {
                if (!InWRange(targetR))
                {
                    _e.Cast(targetR.Position);
                    ForceR();
                    Utility.DelayAction.Add(20, ForceW);
                    Utility.DelayAction.Add(30, () => ForceCastQ(targetR));
                }
            }
            else if (!UseLogic && _r.IsReady() && _r.Instance.Name == IsFirstR && _w.IsReady() && targetR != null &&
                     _e.IsReady() && targetR.IsValidTarget() && !targetR.IsZombie && (IsKillableR(targetR) || AlwaysR))
            {
                if (!InWRange(targetR))
                {
                    _e.Cast(targetR.Position);
                    ForceR();
                    Utility.DelayAction.Add(20, ForceW);
                }
            }
            else if (UseLogic && _w.IsReady() && _e.IsReady())
            {
                if (targetR.IsValidTarget() && targetR != null && !targetR.IsZombie && !InWRange(targetR))
                {
                    _e.Cast(targetR.Position);
                    Utility.DelayAction.Add(100, ForceItem);
                    Utility.DelayAction.Add(200, ForceW);
                    Utility.DelayAction.Add(30, () => ForceCastQ(targetR));
                }
            }
            else if (!UseLogic && _w.IsReady() && targetR != null && _e.IsReady())
            {
                if (targetR.IsValidTarget() && !targetR.IsZombie && !InWRange(targetR))
                {
                    _e.Cast(targetR.Position);
                    Utility.DelayAction.Add(100, ForceItem);
                    Utility.DelayAction.Add(100, ForceW);
                    Utility.DelayAction.Add(30, () => ForceCastQ(targetR));
                }
            }
            else if (_e.IsReady())
            {
                if (targetR != null && (targetR.IsValidTarget() && !targetR.IsZombie && !InWRange(targetR)))
                {
                    _e.Cast(targetR.Position);
                }
            }
        }

        private static void Burst()
        {
            var target = TargetSelector.GetSelectedTarget();
            if (DoIgnite)
            {
                if (target.HealthPercent < 25 && Ignite.IsReady())
                    Player.Spellbook.CastSpell(Ignite, target);
            }
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                if (_e.IsReady() && !StunBurst && (Player.Distance(target.Position) <= 250 + Player.AttackRange))
                {
                    _e.Cast(target.Position);
                    _r.Cast();
                    CastTitan();
                    CastYoumoo();
                }
                if (Flash.IsReady() && !StunBurst && _r.IsReady() && _e.IsReady() && _w.IsReady() && _r.Instance.Name == IsFirstR &&
                        (Player.Distance(target.Position) <= 800))
                {
                    _e.Cast(target.Position);
                    ForceR();
                    CastYoumoo();
                    Utility.DelayAction.Add(180, FlashW);
                }
                if (Flash.IsReady() && StunBurst && _r.IsReady() && _q.IsReady() && _e.IsReady() && _w.IsReady() &&
                        _r.Instance.Name == IsFirstR &&
                        (Player.Distance(target.Position) <= 1300))
                {
                    _q.Cast(target.Position);
                    if (_qStack == 3 && (Geometry.Distance(Player, target.Position) <= 800))
                    {
                        _e.Cast(target.Position);
                        ForceR();
                        CastYoumoo();
                        Utility.DelayAction.Add(180, FlashW);
                        _q3.Cast(target.Position);




                    }
                }
            }
        }

        private static void FastHarass()
        {
            if (_q.IsReady() && _e.IsReady())
            {
                var target = TargetSelector.GetTarget(450 + Player.AttackRange + 70, TargetSelector.DamageType.Physical);
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    if (!Orbwalking.InAutoAttackRange(target) && !InWRange(target)) _e.Cast(target.Position);
                    Utility.DelayAction.Add(10, ForceItem);
                    Utility.DelayAction.Add(170, () => ForceCastQ(target));
                }
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(400, TargetSelector.DamageType.Physical);
            if (_q.IsReady() && _w.IsReady() && _e.IsReady() && _qStack == 1)
            {
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    ForceCastQ(target);
                    Utility.DelayAction.Add(1, ForceW);
                }
            }
            if (_q.IsReady() && _e.IsReady() && _qStack == 3 && !Orbwalking.CanAttack() && Orbwalking.CanMove(5))
            {
                var epos = Player.ServerPosition +
                           (Player.ServerPosition - target.ServerPosition).Normalized() * 300;
                _e.Cast(epos);
                Utility.DelayAction.Add(190, () => _q.Cast(epos));
            }
        }

        private static void Flee()
        {
            var enemy =
                HeroManager.Enemies.Where(
                    hero =>
                        hero.IsValidTarget(Player.HasBuff("RivenFengShuiEngine")
                            ? 70 + 195 + Player.BoundingRadius
                            : 70 + 120 + Player.BoundingRadius) && _w.IsReady());
            var x = Player.Position.Extend(Game.CursorPos, 300);
            var objAiHeroes = enemy as Obj_AI_Hero[] ?? enemy.ToArray();
            if (_w.IsReady() && objAiHeroes.Any()) foreach (var target in objAiHeroes) if (InWRange(target)) _w.Cast();
            if (_q.IsReady() && !Player.IsDashing()) _q.Cast(Game.CursorPos);
            if (_e.IsReady() && !Player.IsDashing()) _e.Cast(x);
        }

        private static void OnPlay(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe) return;

            switch (args.Animation)
            {
                case "Spell1a":
                    _lastQ = Utils.GameTimeTickCount;
                    if (Qstrange && _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None) Game.Say("/d");
                    _qStack = 2;
                    if (_orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None &&
                        _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LastHit &&
                        _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Flee)
                        Utility.DelayAction.Add(Qd * 10 + 1, Reset);
                    break;
                case "Spell1b":
                    _lastQ = Utils.GameTimeTickCount;
                    if (Qstrange && _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None) Game.Say("/d");
                    _qStack = 3;
                    if (_orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None &&
                        _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LastHit &&
                        _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Flee)
                        Utility.DelayAction.Add(Qd * 10 + 1, Reset);
                    break;
                case "Spell1c":
                    _lastQ = Utils.GameTimeTickCount;
                    if (Qstrange && _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None) Game.Say("/d");
                    _qStack = 1;
                    if (_orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None &&
                        _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LastHit &&
                        _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Flee)
                        Utility.DelayAction.Add(Qld * 10 + 3, Reset);
                    break;
                case "Spell3":
                    if ((_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Burst ||
                         _orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo ||
                         _orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.FastHarass ||
                         _orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Flee) && Youmu) CastYoumoo();
                    break;
                case "Spell4a":
                    _lastR = Utils.GameTimeTickCount;
                    break;
                case "Spell4b":
                    var target = TargetSelector.GetSelectedTarget();
                    if (_q.IsReady() && target.IsValidTarget()) ForceCastQ(target);
                    break;
            }
        }

        private static void OnCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            if (args.SData.Name.Contains("ItemTiamatCleave")) _forceItem = false;
            if (args.SData.Name.Contains("RivenTriCleave")) _forceQ = false;
            if (args.SData.Name.Contains("RivenMartyr")) _forceW = false;
            if (args.SData.Name == IsFirstR) _forceR = false;
            if (args.SData.Name == IsSecondR) _forceR2 = false;
        }

        private static void Reset()
        {
            Player.IssueOrder(GameObjectOrder.MoveTo,
                Player.Position.Extend(Game.CursorPos, Player.Distance(Game.CursorPos) + 10));
            Game.Say("/d");
            Orbwalking.LastAATick = 0;
            Player.IssueOrder(GameObjectOrder.MoveTo,
                Player.Position.Extend(Game.CursorPos, Player.Distance(Game.CursorPos) + 10));
        }

        private static int WRange => Player.HasBuff("RivenFengShuiEngine")
            ? 330
            : 265;

        private static bool InWRange(Obj_AI_Base t) => t != null && t.IsValidTarget(WRange);


        private static bool InQRange(GameObject target)
        {
            return target != null && (Player.HasBuff("RivenFengShuiEngine")
                ? 330 >= Player.Distance(target.Position)
                : 265 >= Player.Distance(target.Position));
        }


        private static void ForceSkill()
        {
            if (_forceQ && _qTarget != null && _qTarget.IsValidTarget(_e.Range + Player.BoundingRadius + 70) &&
                _q.IsReady())
                _q.Cast(_qTarget.Position);
            if (_forceW) _w.Cast();
            if (_forceR && _r.Instance.Name == IsFirstR) _r.Cast();
            if (_forceItem && Items.CanUseItem(Item) && Items.HasItem(Item) && Item != 0) Items.UseItem(Item);
            if (_forceR2 && _r.Instance.Name == IsSecondR)
            {
                var target = TargetSelector.GetSelectedTarget();
                if (target != null) _r.Cast(target.Position);
            }
        }

        private static void ForceItem()
        {
            if (Items.CanUseItem(Item) && Items.HasItem(Item) && Item != 0) _forceItem = true;
            Utility.DelayAction.Add(500, () => _forceItem = false);
        }

        private static void ForceR()
        {
            _forceR = _r.IsReady() && _r.Instance.Name == IsFirstR;
            Utility.DelayAction.Add(500, () => _forceR = false);
        }

        public static void ForceR2()
        {
            _forceR2 = _r.IsReady() && _r.Instance.Name == IsSecondR;
            Utility.DelayAction.Add(500, () => _forceR2 = false);
        }

        private static void ForceW()
        {
            _forceW = _w.IsReady();
            Utility.DelayAction.Add(500, () => _forceW = false);
        }

        private static void ForceCastQ(AttackableUnit target)
        {
            _forceQ = true;
            _qTarget = target;
        }


        private static void FlashW()
        {
            var target = TargetSelector.GetSelectedTarget();
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                Utility.DelayAction.Add(10, () => Player.Spellbook.CastSpell(Flash, target.Position));
            }
        }


        private static bool HasItem()
            => ItemData.Tiamat_Melee_Only.GetItem().IsReady() || ItemData.Ravenous_Hydra_Melee_Only.GetItem().IsReady();

        private static void CastYoumoo()
        {
            if (ItemData.Youmuus_Ghostblade.GetItem().IsReady()) ItemData.Youmuus_Ghostblade.GetItem().Cast();
        }

        private static void OnCasting(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsEnemy && sender.Type == Player.Type && AutoShield)
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
                                if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit &&
                                    !args.SData.Name.Contains("NasusW"))
                                {
                                    if (_e.IsReady()) _e.Cast(epos);
                                }
                            }

                            break;
                        case SpellDataTargetType.SelfAoe:

                            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit)
                            {
                                if (_e.IsReady()) _e.Cast(epos);
                            }

                            break;
                    }
                    if (args.SData.Name.Contains("IreliaEquilibriumStrike"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_w.IsReady() && InWRange(sender)) _w.Cast();
                            else if (_e.IsReady()) _e.Cast(epos);
                        }
                    }
                    if (args.SData.Name.Contains("TalonCutthroat"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_w.IsReady()) _w.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("RenektonPreExecute"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_w.IsReady()) _w.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("GarenRPreCast"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_e.IsReady()) _e.Cast(epos);
                        }
                    }

                    if (args.SData.Name.Contains("GarenQAttack"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_e.IsReady()) _e.Cast();
                        }
                    }

                    if (args.SData.Name.Contains("XenZhaoThrust3"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_w.IsReady()) _w.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("RengarQ"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_e.IsReady()) _e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("RengarPassiveBuffDash"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_e.IsReady()) _e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("RengarPassiveBuffDashAADummy"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_e.IsReady()) _e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("TwitchEParticle"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_e.IsReady()) _e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("FizzPiercingStrike"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_e.IsReady()) _e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("HungeringStrike"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_e.IsReady()) _e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("YasuoDash"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_e.IsReady()) _e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("KatarinaRTrigger"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_w.IsReady() && InWRange(sender)) _w.Cast();
                            else if (_e.IsReady()) _e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("YasuoDash"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_e.IsReady()) _e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("KatarinaE"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_w.IsReady()) _w.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("MonkeyKingQAttack"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_e.IsReady()) _e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("MonkeyKingSpinToWin"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_e.IsReady()) _e.Cast();
                            else if (_w.IsReady()) _w.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("MonkeyKingQAttack"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_e.IsReady()) _e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("MonkeyKingQAttack"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_e.IsReady()) _e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("MonkeyKingQAttack"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (_e.IsReady()) _e.Cast();
                        }
                    }
                }
            }
        }


        private static double Basicdmg(Obj_AI_Base target)
        {
            if (target != null)
            {
                double dmg = 0;
                double passivenhan;
                if (Player.Level >= 18)
                {
                    passivenhan = 0.5;
                }
                else if (Player.Level >= 15)
                {
                    passivenhan = 0.45;
                }
                else if (Player.Level >= 12)
                {
                    passivenhan = 0.4;
                }
                else if (Player.Level >= 9)
                {
                    passivenhan = 0.35;
                }
                else if (Player.Level >= 6)
                {
                    passivenhan = 0.3;
                }
                else if (Player.Level >= 3)
                {
                    passivenhan = 0.25;
                }
                else
                {
                    passivenhan = 0.2;
                }
                if (HasItem()) dmg = dmg + Player.GetAutoAttackDamage(target) * 0.7;
                if (_w.IsReady()) dmg = dmg + _w.GetDamage(target);
                if (_q.IsReady())
                {
                    var qnhan = 4 - _qStack;
                    dmg = dmg + _q.GetDamage(target) * qnhan + Player.GetAutoAttackDamage(target) * qnhan * (1 + passivenhan);
                }
                dmg = dmg + Player.GetAutoAttackDamage(target) * (1 + passivenhan);
                return dmg;
            }
            return 0;
        }


        private static float GetComboDamage(Obj_AI_Base enemy)
        {
            if (enemy != null)
            {
                float damage = 0;
                float passivenhan;
                if (Player.Level >= 18)
                {
                    passivenhan = 0.5f;
                }
                else if (Player.Level >= 15)
                {
                    passivenhan = 0.45f;
                }
                else if (Player.Level >= 12)
                {
                    passivenhan = 0.4f;
                }
                else if (Player.Level >= 9)
                {
                    passivenhan = 0.35f;
                }
                else if (Player.Level >= 6)
                {
                    passivenhan = 0.3f;
                }
                else if (Player.Level >= 3)
                {
                    passivenhan = 0.25f;
                }
                else
                {
                    passivenhan = 0.2f;
                }
                if (HasItem()) damage = damage + (float)Player.GetAutoAttackDamage(enemy) * 0.7f;
                if (_w.IsReady()) damage = damage + _w.GetDamage(enemy);
                if (_q.IsReady())
                {
                    var qnhan = 4 - _qStack;
                    damage = damage + _q.GetDamage(enemy) * qnhan +
                             (float)Player.GetAutoAttackDamage(enemy) * qnhan * (1 + passivenhan);
                }
                damage = damage + (float)Player.GetAutoAttackDamage(enemy) * (1 + passivenhan);
                if (_r.IsReady())
                {
                    return damage * 1.2f + _r.GetDamage(enemy);
                }

                return damage;
            }
            return 0;
        }

        public static bool IsKillableR(Obj_AI_Hero target)
        {
            return RKillable && target.IsValidTarget() && Totaldame(target) >= target.Health &&
                   Basicdmg(target) <= target.Health ||
                   Player.CountEnemiesInRange(900) >= 2 && !target.HasBuff("kindrednodeathbuff") &&
                   !target.HasBuff("Undying Rage") && !target.HasBuff("JudicatorIntervention");
        }


        private static double Totaldame(Obj_AI_Base target)
        {
            if (target == null) return 0;
            double dmg = 0;
            double passivenhan;
            if (Player.Level >= 18)
                passivenhan = 0.5;
            else if (Player.Level >= 15)
                passivenhan = 0.45;
            else if (Player.Level >= 12)
                passivenhan = 0.4;
            else if (Player.Level >= 9)
                passivenhan = 0.35;
            else if (Player.Level >= 6)
                passivenhan = 0.3;
            else if (Player.Level >= 3)
                passivenhan = 0.25;
            else
                passivenhan = 0.2;
            if (HasItem()) dmg = dmg + Player.GetAutoAttackDamage(target) * 0.7;
            if (_w.IsReady()) dmg = dmg + _w.GetDamage(target);
            if (_q.IsReady())
            {
                var qnhan = 4 - _qStack;
                dmg = dmg + _q.GetDamage(target) * qnhan + Player.GetAutoAttackDamage(target) * qnhan * (1 + passivenhan);
            }
            dmg = dmg + Player.GetAutoAttackDamage(target) * (1 + passivenhan);
            if (!_r.IsReady()) return dmg;
            var rdmg = Rdame(target, target.Health - dmg * 1.2);
            return dmg * 1.2 + rdmg;
        }

        private static double Rdame(Obj_AI_Base target, double health)
        {
            if (target == null) return 0;
            var missinghealth = (target.MaxHealth - health) / target.MaxHealth > 0.75
                ? 0.75
                : (target.MaxHealth - health) / target.MaxHealth;
            var pluspercent = missinghealth * 2;
            var rawdmg = new double[] { 80, 120, 160 }[_r.Level - 1] + 0.6 * Player.FlatPhysicalDamageMod;
            return Player.CalcDamage(target, Damage.DamageType.Physical, rawdmg * (1 + pluspercent));
        }
    }
}