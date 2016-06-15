#region

using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

#endregion

namespace Nechrito_Twitch
{
    internal class Program
    {
        private static readonly Obj_AI_Hero Player = ObjectManager.Player;
        private static readonly HpBarIndicator Indicator = new HpBarIndicator();

        private static readonly string[] Monsters =
        {
            "SRU_Red", "SRU_Gromp", "SRU_Krug", "SRU_Razorbeak", "SRU_Murkwolf"
        };

        private static readonly string[] Dragons =
        {
            "SRU_Dragon_Air", "SRU_Dragon_Fire", "SRU_Dragon_Water", "SRU_Dragon_Earth", "SRU_Dragon_Elder", "SRU_Baron",
            "SRU_RiftHerald"
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
                "<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Nechrito Twitch</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Version: 6</font></b>");
            Game.PrintChat(
                "<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Update</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Exploit</font></b>");

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
            var target = TargetSelector.GetTarget(Player.AttackRange, TargetSelector.DamageType.Physical); // Looks for a target within AA Range.
            if (target == null || !target.IsValidTarget() || target.IsInvulnerable) return; //If target isn't defined or a valid target within AA Range or target is in zhonyas, return

            if (!MenuConfig.Exploit) return; // If Exploit in Menu is "Off", return.
            if (!Spells._q.IsReady()) return;// if Twitch's Q isn't ready, return.

            if (Spells._e.IsReady() && MenuConfig.EAA) // If Twitch's E is ready and EAA Menu is "On", do the following code within the brackets.
            {
                if (target.Health <= Player.GetAutoAttackDamage(target)*1.275 + GetDamage(target)) // If our targets healh is less than AA * 1.275 + Twitch's E Damage, do the following
                {
                    if (!target.IsFacing(Player) && target.Distance(Player) >= Player.AttackRange - 50) // Return if our target isn't facing us and he's at AA range - 50
                    {
                        Game.PrintChat("Exploit Will NOT Interrupt Combo!!"); // Prints in chat
                        return; // Will return 
                    }
                    // Here we could use an else statement, but it would be redudant and a waste.
                    Spells._e.Cast(); // Will cast Twitch's E spell
                    Utility.DelayAction.Add(500, () => Game.PrintChat("Casting E to then cast AA Q")); // Delays the message with 0.5 seconds (500 milliseconds)
                }
            }

            if (!(target.Health < Player.GetAutoAttackDamage(target)) || !Player.IsWindingUp) return; // Returns if our targets health is less than AA dmg and we aren't attacking

            Spells._q.Cast(); // Will cast Twitch's Q spell
            do // begins a "do" loop
            {
                // Delays the message with 0.5s 
              Utility.DelayAction.Add(500, () => Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Exploit Active</font></b><b><font color=\"#FFFFFF\">]</font></b>"));
            } while (Spells._q.Cast()); // Will loop this message during the time we cast Q.
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(Spells._w.Range, TargetSelector.DamageType.Physical);
            if (target == null || !target.IsValidTarget() || target.IsInvulnerable) return;

            if (!MenuConfig.UseW) return;
            if (target.Health < Player.GetAutoAttackDamage(target, true)*2) return;
            var wPred = Spells._w.GetPrediction(target).CastPosition;

            if (Spells._w.IsReady())
            {
                Spells._w.Cast(wPred);
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(Spells._e.Range, TargetSelector.DamageType.Physical);
            if (target == null || target.IsDead || !target.IsValidTarget() || target.IsInvulnerable) return;

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

        private static void LaneClear()
        {
            if (MenuConfig.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear) return;

            var minions = MinionManager.GetMinions(Player.ServerPosition, 800);

            if (minions == null) return;

            var wPrediction = Spells._w.GetCircularFarmLocation(minions);

            if (!MenuConfig.LaneW) return;

            if (!Spells._w.IsReady()) return;

            if (wPrediction.MinionsHit >= 4)
            {
                Spells._w.Cast(wPrediction.Position);
            }
        }

        private static void JungleClear()
        {
            if (MenuConfig.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear) return;

            var mobs = MinionManager.GetMinions(Player.Position, Spells._w.Range, MinionTypes.All, MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth);

            var wPrediction = Spells._w.GetCircularFarmLocation(mobs);
            if (mobs.Count == 0) return;

            if (wPrediction.MinionsHit >= 3 && Player.ManaPercent >= 20)
            {
                Spells._w.Cast(wPrediction.Position);
            }

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
                if (!MenuConfig.QRecall) return;
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
            var mob = MinionManager.GetMinions(Spells._e.Range, MinionTypes.All, MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth);

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