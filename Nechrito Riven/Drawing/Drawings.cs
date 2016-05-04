using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System;
using System.Linq;

namespace NechritoRiven
{
   internal class Drawings : Riven
    {
        private static readonly HpBarIndicator Indicator = new HpBarIndicator();
        public static void Drawing_OnEndScene(EventArgs args)                   
        {
            foreach (   
                var enemy in
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(ene => ene.IsValidTarget() && !ene.IsZombie))
            {
                if (MenuConfig.Dind)
                {
                    var ezkill = Spells._r.IsReady() && Dmg.IsLethal(enemy)
                        ? new ColorBGRA(0, 255, 0, 120)
                        : new ColorBGRA(255, 255, 0, 120);
                    Indicator.unit = enemy;
                    Indicator.drawDmg(Dmg.GetComboDamage(enemy), ezkill);
                }
            }
        }
        public static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead)
                return;
            var heropos = Drawing.WorldToScreen(ObjectManager.Player.Position);

            /*if(MenuConfig.FleeSpot)
            {
                if(!Spells._q.IsReady())
                { return; }
                var spot = WallJump.GetNearest(Player.ServerPosition);
                Render.Circle.DrawCircle(spot.Start, 100, _qstack == 3 ? System.Drawing.Color.White : System.Drawing.Color.Red);
                Render.Circle.DrawCircle(spot.End, 100, _qstack == 3 ? System.Drawing.Color.White : System.Drawing.Color.Red);
            }*/

            if (MenuConfig.DrawCb)
                Render.Circle.DrawCircle(Player.Position, 250 + Player.AttackRange + 70,
                    Spells._e.IsReady() ? System.Drawing.Color.FromArgb(120, 0, 170, 255) : System.Drawing.Color.IndianRed);
            if (MenuConfig.DrawBt && Spells.Flash != SpellSlot.Unknown)
                Render.Circle.DrawCircle(Player.Position, 750,
                    Spells._r.IsReady() && Spells.Flash.IsReady()
                        ? System.Drawing.Color.FromArgb(120, 0, 170, 255)
                        : System.Drawing.Color.IndianRed);

            if (MenuConfig.DrawFh)
                Render.Circle.DrawCircle(Player.Position, 450 + Player.AttackRange + 70,
                    Spells._e.IsReady() && Spells._q.IsReady()
                        ? System.Drawing.Color.FromArgb(120, 0, 170, 255)
                        : System.Drawing.Color.IndianRed);
            if (MenuConfig.DrawHs)
                Render.Circle.DrawCircle(Player.Position, 400,
                    Spells._q.IsReady() && Spells._w.IsReady()
                        ? System.Drawing.Color.FromArgb(120, 0, 170, 255)
                        : System.Drawing.Color.IndianRed);
            if (MenuConfig.DrawAlwaysR)
            {
                Drawing.DrawText(heropos.X - 15, heropos.Y + 20, System.Drawing.Color.DodgerBlue, "Force R  (     )");
                Drawing.DrawText(heropos.X + 53, heropos.Y + 20,
                    MenuConfig.AlwaysR ? System.Drawing.Color.LimeGreen : System.Drawing.Color.Red, MenuConfig.AlwaysR ? "On" : "Off");
            }
            if (MenuConfig.ForceFlash)
            {
                Drawing.DrawText(heropos.X - 15, heropos.Y + 40, System.Drawing.Color.DodgerBlue, "Force Flash  (     )");
                Drawing.DrawText(heropos.X + 83, heropos.Y + 40,
                    MenuConfig.AlwaysF ? System.Drawing.Color.LimeGreen : System.Drawing.Color.Red, MenuConfig.AlwaysF ? "On" : "Off");
            }
        }
  }
}
