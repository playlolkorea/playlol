using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

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
            Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Nechrito Gragas</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Version: 1 (Date: 4/2-16)</font></b>");
            MenuConfig.LoadMenu();
            Spells.Initialise();
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
                case Orbwalking.OrbwalkingMode.CustomMode:
                    Mode.InsecLogic();
                    break;
            }
        }
        private static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (args.Target is Obj_AI_Minion)
            {
                if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
                {
                    var minions = MinionManager.GetMinions(Player.ServerPosition, 600f).FirstOrDefault();
                    {
                        if (minions == null)
                            return;
                        if (Spells._w.IsReady() && MenuConfig.LaneW)
                            Spells._w.Cast();
                        if (Spells._e.IsReady() && MenuConfig.LaneE)
                            Spells._e.Cast(GetCenterMinion());
                        if (Spells._q.IsReady() && MenuConfig.LaneQ)
                            Spells._q.Cast(GetCenterMinion());
                    }
                   
                }
            }
        }
        
        public static Obj_AI_Base GetCenterMinion()
        {
            var minionposition = MinionManager.GetMinions(300 + Spells._q.Range).Select(x => x.Position.To2D()).ToList();
            var center = MinionManager.GetBestCircularFarmLocation(minionposition, 250, 300 + Spells._q.Range);

            return center.MinionsHit >= 3
                ? MinionManager.GetMinions(1000).OrderBy(x => x.Distance(center.Position)).FirstOrDefault()
                : null;
        }
        private static void Killsteal()
        {
            if(Spells._q.IsReady() && Spells._r.IsReady())
            if (Spells._q.IsReady())
                {
                    var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._q.Range) && !x.IsZombie);
                    foreach (var target in targets)
                    {
                        if (target.Health < Spells._q.GetDamage(target) + Spells._q.GetDamage(target))
                            
                        {
                            Spells._q.Cast(target);
                            Spells._r.Cast(target);
                        }
                    }
                }
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._q.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Spells._q.GetDamage(target))
                        Spells._q.Cast(target);
                }
            }
            if (Spells._e.IsReady())
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._e.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Spells._e.GetDamage(target))
                        Spells._e.Cast(target);
                }
            }
            if (Spells._r.IsReady())
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._r.Range + Spells._e.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Spells._r.GetDamage(target) && !target.IsInvulnerable && (Player.Distance(target.Position) <= Spells._e.Range) && (Player.Distance(target.Position) >= Spells._r.Range))
                    {
                        Spells._e.Cast(target);
                        Utility.DelayAction.Add(60, () => Spells._r.Cast(target));
                    }
                }
            }
        }
        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead)
                return;
            var heropos = Drawing.WorldToScreen(ObjectManager.Player.Position);
        }
        private static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (
                var enemy in
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(ene => ene.IsValidTarget() && !ene.IsZombie))
            {
                if (MenuConfig.dind)
                {
                    var ezkill = Spells._r.IsReady() && Dmg.IsLethal(enemy)
                        ? new ColorBGRA(0, 255, 0, 120)
                        : new ColorBGRA(255, 255, 0, 120);
                    Indicator.unit = enemy;
                    Indicator.drawDmg(Dmg.ComboDmg(enemy), ezkill);
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
