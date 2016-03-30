using System;
using LeagueSharp.Common;

namespace NechritoRiven
{
    class Jungle
    {
        private static void Game_OnUpdate(EventArgs args)
        {
            JungleLogic();
        }
        public static void JungleLogic()
        {
            var mobs = MinionManager.GetMinions(190 + Program.Player.AttackRange, MinionTypes.All, MinionTeam.Neutral,
               MinionOrderTypes.MaxHealth);

            if (mobs.Count <= 0)
                return;

            if (Spells._e.IsReady() && !Orbwalking.InAutoAttackRange(mobs[0]))
            {
                Spells._e.Cast(mobs[0].Position);
            }
        }
    }
}
