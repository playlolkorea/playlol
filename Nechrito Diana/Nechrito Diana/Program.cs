using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SPrediction;
using SharpDX;

namespace Nechrito_Diana
{
    class Program
    {
        /// <summary>
        /// To Do
        /// Auto Zhonyas
        /// 
        /// </summary>
        public static readonly Obj_AI_Hero Player = ObjectManager.Player;
        private static readonly HpBarIndicator Indicator = new HpBarIndicator();
        private static Orbwalking.Orbwalker _orbwalker;
        public static Menu Menu;
        private static void Main() => CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        static void Game_OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Diana") return;
            Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Nechrito Diana</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Version: 1</font></b>");
            //Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Update</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\">UPDATE HERE</font></b>");
            MenuConfig.LoadMenu();
            Spells.Initialise();
            Spells.Ignite = Player.GetSpellSlot("summonerdot");

            Game.OnUpdate += OnTick;
            Interrupter2.OnInterruptableTarget += interrupt;
            AntiGapcloser.OnEnemyGapcloser += gapcloser;
            Drawing.OnEndScene += Drawing_OnEndScene;
        }
        private static void OnTick(EventArgs args)
        {
            Logic.SmiteCombo();
            Logic.SmiteJungle();
            // Killsteal, Switch to ComboMode Etc.
            switch(MenuConfig._orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Modes.ComboLogic();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    Modes.LaneLogic();
                    Modes.JungleLogic();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Modes.HarassLogic();
                    break;
            }
        }
        public static void Heal()
        {
            if (Player.IsRecalling())
                return;
           if(Player.HealthPercent <= MenuConfig.AutoW.MinValue && Spells._w.IsReady())
            {
                Spells._w.Cast();
            }
        }
        public static void Killsteal()
        {
            if (Spells._q.IsReady() && MenuConfig.ksQ)
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._q.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Spells._r.GetDamage(target) && !target.IsInvulnerable && (Player.Distance(target.Position) <= Spells._q.Range))
                    {
                        Spells._q.Cast(target);
                    }
                }
            }
            if(Spells._r.IsReady() && MenuConfig.ksR)
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._r.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Spells._r.GetDamage(target) && !target.IsInvulnerable && (Player.Distance(target.Position) <= Spells._q.Range))
                    {
                        Spells._r.Cast(target);
                    }
                }
            }
            if (Spells._r.IsReady() && Spells._q.IsReady() && MenuConfig.ksR && MenuConfig.ksQ)
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._r.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Spells._r.GetDamage(target) + Spells._q.GetDamage(target) && !target.IsInvulnerable && (Player.Distance(target.Position) <= Spells._q.Range))
                    {
                        Spells._q.Cast(target);
                        Spells._r.Cast(target);
                    }
                }
            }
            if (Spells.Ignite.IsReady() && MenuConfig.ignite)
            {
                var target = TargetSelector.GetTarget(600f, TargetSelector.DamageType.True);
                if (target.IsValidTarget(600f) && Dmg.IgniteDamage(target) >= target.Health)
                {
                    Player.Spellbook.CastSpell(Spells.Ignite, target);
                }
            }
            if (Logic.Smite.IsReady() && MenuConfig.ksSmite)
            {
                var target = TargetSelector.GetTarget(600f, TargetSelector.DamageType.True);
                if (target.IsValidTarget(600f) && Dmg.SmiteDamage(target) >= target.Health)
                {
                    Player.Spellbook.CastSpell(Logic.Smite, target);
                }
            }
        }

        private static void interrupt(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (sender.IsEnemy && Spells._e.IsReady() && sender.IsValidTarget() && !sender.IsZombie && MenuConfig.Interrupt)
            {
                if (sender.IsValidTarget(Spells._e.Range + sender.BoundingRadius)) Spells._e.Cast();
            }
        }
        private static void gapcloser(ActiveGapcloser gapcloser)
        {
            var target = gapcloser.Sender;
            if (target.IsEnemy && Spells._e.IsReady() && target.IsValidTarget() && !target.IsZombie && MenuConfig.Gapcloser)
            {
                if (target.IsValidTarget(Spells._e.Range + Player.BoundingRadius + target.BoundingRadius)) Spells._e.Cast();
            }
        }
        private static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget() && !ene.IsZombie))
            {
                var EasyKill = Spells._r.IsReady() && Spells._r.IsReady() && Dmg.IsLethal(enemy)
                       ? new ColorBGRA(0, 255, 0, 120)
                       : new ColorBGRA(255, 255, 0, 120);
                    Indicator.unit = enemy;
                    Indicator.drawDmg(Dmg.ComboDmg(enemy), EasyKill);
            }
        }
    }
}
