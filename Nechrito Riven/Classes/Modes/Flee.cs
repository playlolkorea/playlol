using System;
using LeagueSharp.Common;
using LeagueSharp;
using System.Linq;

namespace NechritoRiven
{
    class Flee
    {
        private static void Game_OnUpdate(EventArgs args)
        {
            FleeLogic();
        }
        public static void FleeLogic()
        {
            var enemy =
               HeroManager.Enemies.Where(
                   hero =>
                       hero.IsValidTarget(Program.Player.HasBuff("RivenFengShuiEngine")
                           ? 70 + 195 + Program.Player.BoundingRadius
                           : 70 + 120 + Program.Player.BoundingRadius) && Spells._w.IsReady());
            var x = Program.Player.Position.Extend(Game.CursorPos, 300);
            var objAiHeroes = enemy as Obj_AI_Hero[] ?? enemy.ToArray();
            if (Spells._w.IsReady() && objAiHeroes.Any()) foreach (var target in objAiHeroes) if (Logic.InWRange(target)) Spells._w.Cast();
            if (Spells._q.IsReady() && !Program.Player.IsDashing()) Spells._q.Cast(Game.CursorPos);
            if (Spells._e.IsReady() && !Program.Player.IsDashing()) Spells._e.Cast(x);
        }
    }
}
