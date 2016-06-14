using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;


namespace Nechrito_Twitch
{
    internal class Program
    {
        private static readonly Obj_AI_Hero Player = ObjectManager.Player;
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
                "<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Nechrito Twitch</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Version: 5½</font></b>");
            Game.PrintChat(
                "<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Update</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Rework & Exploit</font></b>");

            
            Recall();
            Drawing.OnEndScene += Drawing_OnEndScene;
            Game.OnUpdate += Game_OnUpdate;
            MenuConfig.LoadMenu();
            Spells.Initialise();
        }

        private static void Game_OnUpdate(EventArgs args)
        {
           AutoE();
           Exploit();

            switch (MenuConfig.Orbwalker.ActiveMode)
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

        private static void Exploit()
        {
            var target = TargetSelector.GetTarget(Player.AttackRange, TargetSelector.DamageType.Physical);
            if (target == null || !target.IsValidTarget() || target.IsInvulnerable) return;

            if (!MenuConfig.Exploit) return;
            if (!Spells._q.IsReady()) return;

            if (Spells._e.IsReady() && MenuConfig.EAA)
            {
                if (!target.IsFacing(Player))
                {
                    Game.PrintChat("Target is not facing, will now return");
                    return;
                }
                if (target.Distance(Player) >= Player.AttackRange)
                {
                    Game.PrintChat("Out of AA Range, will now return");
                    return;
                }

                if (target.Health <= Player.GetAutoAttackDamage(target) *1.33 + GetDamage(target))
                {
                    Spells._e.Cast();
                    Game.PrintChat("Casting E to then cast AA Q");
                }
            }

            if (target.Health < Player.GetAutoAttackDamage(target, true) && Player.IsWindingUp)
            {
                Spells._q.Cast();
                do
                {
                    Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Exploit Active</font></b><b><font color=\"#FFFFFF\">]</font></b>");
                    Game.PrintChat("Casting Q");
                } while (Spells._q.Cast());
            }
           
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(Spells._w.Range, TargetSelector.DamageType.Physical);
            if (target == null || !target.IsValidTarget() || target.IsInvulnerable) return;

            

            if (!MenuConfig.UseW) return;
            if (target.Health < Player.GetAutoAttackDamage(target, true) * 2) return;
            var wPred = Spells._w.GetPrediction(target).CastPosition;

            if (Spells._w.IsReady())
            {
                Spells._w.Cast(wPred);
            }
        }

        public static void Harass()
        {
            var target = TargetSelector.GetTarget(Spells._e.Range, TargetSelector.DamageType.Physical);

            if (!Orbwalking.InAutoAttackRange(target) && target.GetBuffCount("twitchdeadlyvenom") >= MenuConfig.ESlider && Player.ManaPercent >= 50 && Spells._e.IsReady())
            {
                Spells._e.Cast();
            }

            if (!MenuConfig.HarassW) return;
            var wPred = Spells._w.GetPrediction(target).CastPosition;

            if (target.IsValidTarget(Spells._w.Range) && Spells._w.IsReady())
            {
                Spells._w.Cast(wPred);
            }
        }

        public static void LaneClear()
        {
            if (MenuConfig.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear) return;

            var minions = MinionManager.GetMinions(Player.ServerPosition, 800);

            if (minions == null) return;

            var wPrediction = Spells._w.GetCircularFarmLocation(minions);

            if (!MenuConfig.LaneW) return;

            if (Spells._w.IsReady())
            {
                if(wPrediction.MinionsHit >= 4)
                Spells._w.Cast(wPrediction.Position);
            }
        }

        public static void JungleClear()
        {
            if (MenuConfig.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear) return;

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
            Spellbook.OnCastSpell += (sender, eventArgs) =>
            {
                if(!MenuConfig.QRecall) return;
                if (!Spells._q.IsReady() || !Spells._recall.IsReady()) return;
                if (eventArgs.Slot != SpellSlot.Recall) return;

                Spells._q.Cast();
                Utility.DelayAction.Add((int) Spells._q.Delay + 300,
                    () => ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Recall));
                eventArgs.Process = false;
            };
        }


        private static void AutoE()
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
                    if (m.CharData.BaseSkinName.Contains("SRU_Red") && MenuConfig.StealBuff) continue;
                    if (Spells._e.IsKillable(m))
                        Spells._e.Cast();
                }
            }
            
            if (!MenuConfig.KsE) return;

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsValidTarget(Spells._e.Range) && Spells._e.IsKillable(enemy)))
            {
                Spells._e.Cast(enemy);
            }
        }


        private static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget() && !ene.IsZombie))
            {
                if (!MenuConfig.Dind) continue;

                Indicator.unit = enemy;
                Indicator.drawDmg(GetDamage(enemy), new ColorBGRA(255, 204, 0, 170));
            }
        }
    }
}
