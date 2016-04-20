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
        /// Interrupt Tp Etc.
        /// Auto Zhonyas
        /// Auto Shield
        /// Add JungleLogic, Laneclear Etc.
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
            Game.OnUpdate += OnTick;
            Interrupter2.OnInterruptableTarget += interrupt;
            AntiGapcloser.OnEnemyGapcloser += gapcloser;
            Drawing.OnEndScene += Drawing_OnEndScene;
        }
        private static void OnTick(EventArgs args)
        {
            // Killsteal, Switch to ComboMode Etc.
            switch(MenuConfig._orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Modes.ComboLogic();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    Modes.JungleLogic();
                    break;
            }
        }
        public static void Heal()
        {
            if (Player.IsRecalling())
                return;
            // HEAL CODE HERE!!!
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
