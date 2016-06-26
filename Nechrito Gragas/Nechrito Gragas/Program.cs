using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SPrediction;

namespace Nechrito_Gragas
{
    class Program
    {
        public static readonly int[] BlueSmite = { 3706, 1400, 1401, 1402, 1403 };

        public static readonly int[] RedSmite = { 3715, 1415, 1414, 1413, 1412 };

        private static Orbwalking.Orbwalker _orbwalker;

        public static Obj_AI_Hero Player => ObjectManager.Player;

        private static readonly HpBarIndicator Indicator = new HpBarIndicator();

        private static void Main() => CustomEvents.Game.OnGameLoad += OnGameLoad;

        private static void OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Gragas") return;

            Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Nechrito Gragas</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Version: 4 (Date: 26/6-16)</font></b>");

            MenuConfig.LoadMenu();
            Spells.Initialise();

            Game.OnUpdate += Mode.Game_OnUpdate;
            Game.OnUpdate += OnTick;

            Obj_AI_Base.OnDoCast += OnDoCast;

            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnEndScene += Drawing_OnEndScene;
        }
        private static void OnTick(EventArgs args)
        {
            SmiteJungle();
            SmiteCombo();
            Killsteal();

            switch (MenuConfig._orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Mode.ComboLogic();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    Mode.JungleLogic();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Mode.HarassLogic();
                    break;
            }
        }
        private static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (Player.IsWindingUp) return;

            if (args.Target is Obj_AI_Minion)
            {
                if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
                {
                    var minions = MinionManager.GetMinions(Player.ServerPosition, 600);
                    {
                        if (minions == null)
                            return;

                        foreach (var m in minions)
                        {
                            if (Spells.E.IsReady() && MenuConfig.LaneE)
                            {
                                if (m.Health < Spells.E.GetDamage(m))
                                {
                                    Spells.E.Cast(GetCenterMinion());
                                }
                            }

                            if (Spells.Q.IsReady() && MenuConfig.LaneQ)
                            {
                                if (m.Health < Spells.Q.GetDamage(m))
                                {
                                    Spells.Q.Cast(GetCenterMinion());
                                }
                            }
                        }

                        if (Spells.W.IsReady() && MenuConfig.LaneW)
                        {
                            Spells.W.Cast();
                        }
                    }
                }
            }
        }
    
        
        public static Obj_AI_Base GetCenterMinion()
        {
            var minionposition = MinionManager.GetMinions(300 + Spells.Q.Range).Select(x => x.Position.To2D()).ToList();
            var center = MinionManager.GetBestCircularFarmLocation(minionposition, 250, 300 + Spells.Q.Range);

            return center.MinionsHit >= 4
                ? MinionManager.GetMinions(1000).OrderBy(x => x.Distance(center.Position)).FirstOrDefault()
                : null;
        }
        private static void Killsteal()
        {
            if (Spells.Q.IsReady() && Spells.R.IsReady())
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells.Q.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Spells.Q.GetDamage(target) + Spells.Q.GetDamage(target))
                    {
                        var pos = Spells.R.GetSPrediction(target).CastPosition + 60;

                        Spells.Q.Cast(pos);
                        Spells.R.Cast(pos);
                    }
                }
            }

            if(Spells.Q.IsReady())
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells.Q.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Spells.Q.GetDamage(target))
                    {
                        var pos = Spells.Q.GetSPrediction(target).CastPosition;
                        Spells.Q.Cast(pos);
                        Spells.Q.Cast(pos);
                    }
                }
            }

            if (Spells.E.IsReady())
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells.E.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Spells.E.GetDamage(target))
                    {
                        var pos = Spells.E.GetSPrediction(target).CastPosition;
                        Spells.E.Cast(pos);
                    }
                }
            }
        }
        
        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead || !MenuConfig.prediction) return;

            var Target = TargetSelector.GetSelectedTarget();

            if(Target != null)
            {
                Render.Circle.DrawCircle(Mode.pred(Target), 100, System.Drawing.Color.GhostWhite);
            }
           
        }
        private static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget() && !ene.IsZombie))
            {
                if (MenuConfig.dind)
                {
                    var lethal = Spells.R.IsReady() && Dmg.IsLethal(enemy)
                        ? new ColorBGRA(0, 255, 0, 120)
                        : new ColorBGRA(255, 255, 0, 120);

                    Indicator.unit = enemy;
                    Indicator.drawDmg(Dmg.ComboDmg(enemy), lethal);
                }
            }
        }
        protected static void SmiteCombo()
        {
            if (BlueSmite.Any(id => Items.HasItem(id)))
            {
                Spells.Smite = Player.GetSpellSlot("s5_summonersmiteplayerganker");
                return;
            }

            if (RedSmite.Any(id => Items.HasItem(id)))
            {
                Spells.Smite = Player.GetSpellSlot("s5_summonersmiteduel");
                return;
            }

            Spells.Smite = Player.GetSpellSlot("summonersmite");
        }

        protected static void SmiteJungle()
        {
            foreach (var minion in MinionManager.GetMinions(900f, MinionTypes.All, MinionTeam.Neutral))
            {
                var damage = Player.Spellbook.GetSpell(Spells.Smite).State == SpellState.Ready
                    ? (float)Player.GetSummonerSpellDamage(minion, Damage.SummonerSpell.Smite)
                    : 0;
                if (minion.Distance(Player.ServerPosition) <= 550)
                {
                    if ((minion.CharData.BaseSkinName.Contains("Dragon") || minion.CharData.BaseSkinName.Contains("Baron")))
                    {
                        if (damage >= minion.Health)
                            Player.Spellbook.CastSpell(Spells.Smite, minion);
                    }
                }

            }

        }
    }
}
