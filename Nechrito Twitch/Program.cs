using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;


namespace Nechrito_Twitch
{
    internal class Program
    {
        private static readonly Obj_AI_Hero Player = ObjectManager.Player;
        public static Menu Menu;
        private static Orbwalking.Orbwalker _orbwalker;
        private static readonly HpBarIndicator Indicator = new HpBarIndicator();

        private static readonly string[] Monsters =
        {
           "SRU_Red", "SRU_Gromp", "SRU_Krug","SRU_Razorbeak","SRU_Murkwolf"
        };

        private static readonly string[] Dragons =
        {
            "SRU_Dragon_Air","SRU_Dragon_Fire","SRU_Dragon_Water","SRU_Dragon_Earth","SRU_Dragon_Elder","SRU_Baron","SRU_RiftHerald"
        };
        private static float GetDamage(Obj_AI_Base target)
        {
            return Spells._e.GetDamage(target);
        }
        private static void Main() => CustomEvents.Game.OnGameLoad += OnGameLoad;

        private static void OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Twitch") return;
            Game.PrintChat(
                "<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Nechrito Twitch</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Version: 3</font></b>");
            Game.PrintChat(
                "<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Update</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Rework</font></b>");


            Drawing.OnEndScene += Drawing_OnEndScene;
            Game.OnUpdate += Game_OnUpdate;
            MenuConfig.LoadMenu();
            Spells.Initialise();
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            AutoE();
            Recall();

            switch (MenuConfig._orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Harass();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    LaneClear();
                    JungleClear();
                    break;
            }
        }

        public static void Combo()
        {
            var target = TargetSelector.GetTarget(Spells._e.Range, TargetSelector.DamageType.Physical);
            if (target == null || !target.IsValidTarget() || target.IsDead || target.IsInvulnerable) return;

            if (!MenuConfig.UseW) return;
            if (target.IsValidTarget(Spells._w.Range) && Spells._w.CanCast(target))
            {
                Spells._w.Cast(target);
            }
        }

        public static void Harass()
        {
            var target = TargetSelector.GetTarget(Spells._e.Range, TargetSelector.DamageType.Physical);
            if (!Orbwalking.InAutoAttackRange(target) && target.GetBuffCount("twitchdeadlyvenom") >= MenuConfig.ESlider && Player.ManaPercent >= 50 && Spells._e.IsReady())
            {
                Spells._e.Cast(target);
            }
            if (!MenuConfig.harassW) return;
            if (target.IsValidTarget(Spells._w.Range) && Spells._w.CanCast(target))
            {
                Spells._w.Cast(target);
            }
        }

        public static void LaneClear()
        {
            if (MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear) return;

            var minions = MinionManager.GetMinions(Player.ServerPosition, 800);

            if (minions == null) return;

            var wPrediction = Spells._w.GetCircularFarmLocation(minions);

            if (!MenuConfig.laneW) return;

            if (Spells._w.IsReady())
            {
                Spells._w.Cast(wPrediction.Position);
            }
        }

        public static void JungleClear()
        {
            if (MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear) return;

            var mobs = MinionManager.GetMinions(Player.Position, Spells._w.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            var wPrediction = Spells._w.GetCircularFarmLocation(mobs);
            if (mobs.Count == 0) return;

            Spells._w.Cast(wPrediction.Position);

            foreach (var m in ObjectManager.Get<Obj_AI_Base>().Where(x => Monsters.Contains(x.CharData.BaseSkinName) && !x.IsDead))
            {
                if (m.Health < Spells._e.GetDamage(m))
                {
                    Spells._e.Cast(m);
                }
            }
        }

        private static void Recall()
        {
            if (!MenuConfig.QRecall) return;
            if (!Spells._q.IsReady() || !Spells._recall.IsReady()) return;

            Spells._q.Cast();
            Utility.DelayAction.Add(200, () => Spells._recall.Cast());
        }

        public static void AutoE()
        {
            var mob = MinionManager.GetMinions(Spells._e.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (MenuConfig.StealEpic)
            {
                foreach (var m in ObjectManager.Get<Obj_AI_Base>().Where(x => Dragons.Contains(x.CharData.BaseSkinName) && !x.IsDead))
                {
                    if (m.Health < Spells._e.GetDamage(m))
                    {
                        Spells._e.Cast(m);
                    }
                }
            }

            if (MenuConfig.StealBuff)
            {
                foreach (var m in mob)
                {
                    if (!m.CharData.BaseSkinName.Contains("SRU_Blue") && (!m.CharData.BaseSkinName.Contains("SRU_Red") || !MenuConfig.StealBuff)) continue;
                    if (Spells._e.IsKillable(m))
                        Spells._e.Cast();
                }
            }
            
            if (!MenuConfig.KsE) return;
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsValidTarget(Spells._e.Range) && Spells._e.IsKillable(enemy)))
            { Spells._e.Cast(enemy);}
        }


        private static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget() && !ene.IsZombie))
            {
                if (!MenuConfig.dind) continue;

                Indicator.unit = enemy;
                Indicator.drawDmg(GetDamage(enemy), new ColorBGRA(255, 204, 0, 170));
            }
        }
    }
}
