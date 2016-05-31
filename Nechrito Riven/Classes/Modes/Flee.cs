using System;
using LeagueSharp.Common;
using LeagueSharp;
using System.Linq;
using SharpDX;

namespace NechritoRiven
{
    class Flee : Core
    {
        private static void Game_OnUpdate(EventArgs args)
        {
            FleeLogic();
        }
        public static void FleeLogic()
        {
            if (!MenuConfig.WallFlee)
            {
                return;
            }

            var end = Player.ServerPosition.Extend(Game.CursorPos, Spells.Q.Range);
            var IsWallDash = FleeLOGIC.IsWallDash(end, Spells.Q.Range);

            var Eend = Player.ServerPosition.Extend(Game.CursorPos, Spells.E.Range);
            var WallE = FleeLOGIC.GetFirstWallPoint(Player.ServerPosition, Eend);
            var WallPoint = FleeLOGIC.GetFirstWallPoint(Player.ServerPosition, end);
            Player.GetPath(WallPoint);

            if (Spells.Q.IsReady() && Qstack < 3)
            { Spells.Q.Cast(Game.CursorPos); }


            if (IsWallDash && Qstack == 3 && WallPoint.Distance(Player.ServerPosition) <= 800)
            {
                ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, WallPoint);
                if (WallPoint.Distance(Player.ServerPosition) <= 600)
                {
                    ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, WallPoint);
                    if (WallPoint.Distance(Player.ServerPosition) <= 45)
                    {
                        if (Spells.E.IsReady() )
                        {
                            Spells.E.Cast(WallE);
                        }
                        if (Qstack == 3 && end.Distance(Player.Position) <= 260 && IsWallDash && WallPoint.IsValid())
                        {
                            Player.IssueOrder(GameObjectOrder.MoveTo, WallPoint);
                            Spells.Q.Cast(WallPoint);
                        }

                    }
                }
            }
            /*
            if (!IsWallDash && !MenuConfig.WallFlee)
            {
                var enemy =
             HeroManager.Enemies.Where(
                 hero =>
                     hero.IsValidTarget(Player.HasBuff("RivenFengShuiEngine")
                         ? 70 + 195 + Player.BoundingRadius
                         : 70 + 120 + Player.BoundingRadius) && Spells.W.IsReady());
                var x = Player.Position.Extend(Game.CursorPos, 300);
                var objAiHeroes = enemy as Obj_AI_Hero[] ?? enemy.ToArray();
                if (Spells.Q.IsReady() && !Player.IsDashing()) Spells.Q.Cast(Game.CursorPos);
                if (Spells.W.IsReady() && objAiHeroes.Any()) foreach (var target in objAiHeroes) if (Logic.InWRange(target)) Spells.W.Cast();
                if (Spells.E.IsReady() && !Player.IsDashing()) Spells.E.Cast(x);
            }
            */
        }
    }
}