using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using LeagueSharp;
using LeagueSharp.Common;


namespace Nechrito_Twitch
{
    internal class Program
    {
        private static readonly Obj_AI_Hero Player = ObjectManager.Player;
        public static Menu Menu;
        private static Orbwalking.Orbwalker _orbwalker;

        private static void Main() => CustomEvents.Game.OnGameLoad += OnGameLoad;

        private static void OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Twitch") return;
            Game.PrintChat(
                "<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Nechrito Twitch</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Version: 1</font></b>");
            Game.PrintChat(
                "<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Update</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Release!</font></b>");


            Game.OnUpdate += Game_OnUpdate;
            MenuConfig.LoadMenu();
            Spells.Initialise();
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo) Combo();
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed) Harass();
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear) LaneClear(); JungleClear();
            AutoE();
            //Recall();
        }

        public static void Combo()
        {
            var target = TargetSelector.GetTarget(Spells._e.Range, TargetSelector.DamageType.Physical);
            if (target == null) return;

            if (MenuConfig.UseW)
            {
                if (target.IsValidTarget(Spells._w.Range) && Spells._w.CanCast(target))
                {
                    Spells._w.Cast(target);
                }
            }

            
           

        }

        public static void Harass()
        {
            var target = TargetSelector.GetTarget(Spells._e.Range, TargetSelector.DamageType.Physical);
            if (!Orbwalking.InAutoAttackRange(target) && target.GetBuffCount("twitchdeadlyvenom") >= MenuConfig.ESlider && Player.ManaPercent > 50 && Spells._e.IsReady())
            {
                Spells._e.Cast(target);
            }
            if (MenuConfig.harassW)
            {
                if (target.IsValidTarget(Spells._w.Range) && Spells._w.CanCast(target))
                {
                    Spells._w.Cast(target);
                }
            }
        }
        public static void LaneClear()
        {
            if (MenuConfig.laneW)
             {
                var minions = MinionManager.GetMinions(Player.Position, Spells._w.Range);
                if (minions.Count >= 5)
                    Spells._w.Cast(minions[5].ServerPosition);
             }
            /* Laneclear E Lasthit, Add stacks for enemies + minion lasthit
            var minion = MinionManager.GetMinions(Player.Position, Spells._e.Range);
            foreach (var m in minion)
            {
                if (m.HasBuff("twitchdeadlyvenom") && Spells._e.IsKillable(m))
                        Spells._e.Cast(m);
            }
            */
        }

        public static void JungleClear()
        {
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                var mobs = MinionManager.GetMinions(Player.Position, Spells._w.Range, MinionTypes.All,
                       MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

                if (mobs.Count != 0)
                {
                    Spells._w.Cast(mobs[0].ServerPosition);
                    foreach (var m in mobs)
                    {
                        if (m.HasBuff("twitchdeadlyvenom"))
                        {
                           if(Spells._e.IsKillable(m)) 
                            Spells._e.Cast();
                        }
                    }
                }
            }
        }

        /* Recall Bug with MenuConfig
        private static void Recall()
        {
            if (MenuConfig.QRecall)
            {
                if (Spells._q.IsReady() && Spells._recall.IsReady())
                {
                    Spells._q.Cast();
                    Spells._recall.Cast();
                }
            }
            
        }
        */
        public static void AutoE()
        {
            var mob = MinionManager.GetMinions(Spells._e.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            foreach (var m in mob)
            {
                if ((m.CharData.BaseSkinName.Contains("Dragon") || m.CharData.BaseSkinName.Contains("Baron")))
                    if (Spells._e.IsKillable(m))
                        Spells._e.Cast();

               

            }
            if (MenuConfig.KsE)
            {
                foreach (
                    var enemy in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(enemy => enemy.IsValidTarget(Spells._e.Range) && Spells._e.IsKillable(enemy)))
                    Spells._e.Cast(enemy);

            }
        }
        }
    }
