using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NechritoRiven.Classes.Drawings
{
    class DRAW : Core
    {
        public static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead)
                return;

            var heropos = Drawing.WorldToScreen(ObjectManager.Player.Position);

            var end = Player.ServerPosition.Extend(Game.CursorPos, Spells.Q.Range);
            var IsWallDash = FleeLOGIC.IsWallDash(end, Spells.Q.Range);

            var Eend = Player.ServerPosition.Extend(Game.CursorPos, Spells.E.Range);
            var WallE = FleeLOGIC.GetFirstWallPoint(Player.ServerPosition, Eend);
            var WallPoint = FleeLOGIC.GetFirstWallPoint(Player.ServerPosition, end);

            if (IsWallDash && MenuConfig.FleeSpot)
            {
                if (WallPoint.Distance(Player.ServerPosition) <= 600)
                {
                    Render.Circle.DrawCircle(WallPoint, 60, System.Drawing.Color.White);
                    Render.Circle.DrawCircle(end, 60, System.Drawing.Color.Green);
                }
            }

            if (MenuConfig.DrawCb)
                Render.Circle.DrawCircle(Player.Position, 250 + Player.AttackRange + 70,
                    Spells.E.IsReady() ? System.Drawing.Color.FromArgb(120, 0, 170, 255) : System.Drawing.Color.IndianRed);

            if (MenuConfig.DrawBt && Spells.Flash != SpellSlot.Unknown)
                Render.Circle.DrawCircle(Player.Position, 750,
                    Spells.R.IsReady() && Spells.Flash.IsReady()
                        ? System.Drawing.Color.FromArgb(120, 0, 170, 255)
                        : System.Drawing.Color.IndianRed);

            if (MenuConfig.DrawFh)
                Render.Circle.DrawCircle(Player.Position, 450 + Player.AttackRange + 70,
                    Spells.E.IsReady() && Spells.Q.IsReady()
                        ? System.Drawing.Color.FromArgb(120, 0, 170, 255)
                        : System.Drawing.Color.IndianRed);

            if (MenuConfig.DrawHs)
                Render.Circle.DrawCircle(Player.Position, 400,
                    Spells.Q.IsReady() && Spells.W.IsReady()
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
