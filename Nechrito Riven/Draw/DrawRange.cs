#region

using System;
using System.Drawing;
using LeagueSharp;
using LeagueSharp.Common;
using NechritoRiven.Core;
using NechritoRiven.Menus;

#endregion

namespace NechritoRiven.Draw
{
    internal class DrawRange : Core.Core
    {
        public static void RangeDraw(EventArgs args)
        {
            if (Player.IsDead)
                return;

            var pos = Drawing.WorldToScreen(Player.Position);

            if (MenuConfig.DrawCb)
            {
                Render.Circle.DrawCircle(Player.Position, 250 + Player.AttackRange + 70,
                    Spells.E.IsReady()
                        ? Color.LightBlue
                        : Color.DarkSlateGray);
            }

            if (MenuConfig.DrawBt && Spells.Flash != SpellSlot.Unknown)
            {
                Render.Circle.DrawCircle(Player.Position, 750,
                    Spells.R.IsReady() && Spells.Flash.IsReady()
                        ? Color.LightBlue
                        : Color.DarkSlateGray);
            }

            if (MenuConfig.DrawFh)
            {
                Render.Circle.DrawCircle(Player.Position, 450 + Player.AttackRange + 70,
                 Spells.E.IsReady() && Spells.Q.IsReady()
                      ? Color.LightBlue
                     : Color.DarkSlateGray);
            }
             

            if (MenuConfig.DrawHs)
            {
                Render.Circle.DrawCircle(Player.Position, 400,
                    Spells.Q.IsReady() && Spells.W.IsReady()
                        ? Color.LightBlue
                        : Color.DarkSlateGray);
            }

            if (MenuConfig.DrawAlwaysR)
            {
                Drawing.DrawText(pos.X - 15, pos.Y + 20, System.Drawing.Color.Mauve, "선궁  (     )");
                Drawing.DrawText(pos.X + 53, pos.Y + 20,
                    MenuConfig.AlwaysR ? System.Drawing.Color.White : System.Drawing.Color.Red, MenuConfig.AlwaysR ? "켜짐" : "꺼짐");
            }

            if (!MenuConfig.ForceFlash) return;

            Drawing.DrawText(pos.X - 15, pos.Y + 40, System.Drawing.Color.Mauve, "선플래시  (     )");
            Drawing.DrawText(pos.X + 83, pos.Y + 40, MenuConfig.AlwaysF 
                ? System.Drawing.Color.White
                : System.Drawing.Color.Red,
                MenuConfig.AlwaysF ? "켜짐" : "꺼짐");
        }
    }
}
