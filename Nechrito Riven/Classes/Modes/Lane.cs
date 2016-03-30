using System;
using LeagueSharp.Common;
using LeagueSharp;
using System.Linq;

namespace NechritoRiven
{
    class Lane
    {
        private static readonly Obj_AI_Hero Player = ObjectManager.Player;
        private static void Game_OnUpdate(EventArgs args)
        {
            LaneLogic();
        }
        public static void LaneLogic()
        {
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                var mobs = MinionManager.GetMinions(Player.Position, 600f, MinionTypes.All,
                    MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();

                if (Spells._e.IsReady() && !Orbwalking.InAutoAttackRange(Logic.GetCenterMinion()))
                {
                    Spells._e.Cast(mobs);
                }

                if (Program.HasTitan())
                {
                    Program.CastTitan();
                    return;
                }
                if (Spells._q.IsReady())
                {
                    Logic.ForceItem();
                    Spells._q.Cast(mobs);
                }
                if (Spells._w.IsReady())
                {
                    Logic.ForceItem();
                    Spells._w.Cast(mobs);
                }
            }
        }
    }
}
