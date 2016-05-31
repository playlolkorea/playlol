using System;
using LeagueSharp.Common;
using LeagueSharp;
using SharpDX;
using System.Collections.Generic;
using System.Linq;
using NechritoRiven.Classes.Modes;
using NechritoRiven.Classes.Extras;
using NechritoRiven.Classes.Drawings;

namespace NechritoRiven
{
    class MAIN : Core
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }
        private static void OnLoad(EventArgs args)
        {
            if (Player.ChampionName != "Riven") return;
            Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Nechrito Riven</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Version: 65 (Date: 5/4/2016)</font></b>");
            Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Update</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Q Move</font></b>");

            Timer =
                new Render.Text(
                    "Q Expiry =>  " + ((double)(Logic.LastQ- Utils.GameTimeTickCount + 3800) / 1000).ToString("0.0"),
                    (int)Drawing.WorldToScreen(Player.Position).X - 140,
                    (int)Drawing.WorldToScreen(Player.Position).Y + 10, 30, Color.DodgerBlue, "calibri");
            Timer2 =
                new Render.Text(
                    "R Expiry =>  " + (((double)Logic.LastR- Utils.GameTimeTickCount + 15000) / 1000).ToString("0.0"),
                    (int)Drawing.WorldToScreen(Player.Position).X - 60,
                    (int)Drawing.WorldToScreen(Player.Position).Y + 10, 30, Color.DodgerBlue, "calibri");

            Game.OnUpdate += Modes.Game_OnUpdate;
            Game.OnUpdate += OnUpdate;
            MenuConfig.Load();
            Spells.Load();
            Obj_AI_Base.OnPlayAnimation += Anim.OnPlay;
            Obj_AI_Base.OnProcessSpellCast += Logic.OnCast;
            Game.OnUpdate += Logic.Game_OnUpdate;
            Drawing.OnEndScene += Drawing_OnEndScene;
            Drawing.OnDraw += DRAW.Drawing_OnDraw;
        }
        private static void OnUpdate(EventArgs args)
        {
            //   var target = TargetSelector.GetSelectedTarget();
            //    Game.PrintChat("Buffs: {0}", string.Join(" | ", Player.Buffs.Where(b => b.Caster.NetworkId == Player.NetworkId).Select(b => b.DisplayName)));
            if (Utils.GameTimeTickCount - Logic.LastQ >= 3650 && Qstack != 1 && !Player.IsRecalling() && !Player.InFountain() && MenuConfig.KeepQ && Player.HasBuff("RivenTriCleave") && !Player.Spellbook.IsChanneling && Spells.Q.IsReady()) Spells.Q.Cast(Game.CursorPos);
            
            switch (Orb.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Modes.Combo();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    Modes.JungleClear();
                    Modes.LaneClear();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Modes.Harass();
                    break;
                case Orbwalking.OrbwalkingMode.FastHarass:
                    Modes.FastHarass();
                    break;
                case Orbwalking.OrbwalkingMode.Burst:
                    Modes.Burst();
                    break;
                case Orbwalking.OrbwalkingMode.Flee:
                    Flee.FleeLogic();
                    break;
            }
        }
        private static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget() && !ene.IsZombie))
            {
                if (MenuConfig.Dind)
                {
                    var EasyKill = Spells.Q.IsReady() && Dmg.IsLethal(enemy)
                       ? new ColorBGRA(0, 255, 0, 120)
                       : new ColorBGRA(255, 255, 0, 120);
                    Indicator.unit = enemy;
                    Indicator.drawDmg(Dmg.GetComboDamage(enemy), EasyKill);
                }
            }
        }
    }
}
