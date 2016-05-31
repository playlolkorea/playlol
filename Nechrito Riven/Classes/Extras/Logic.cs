using LeagueSharp;
using LeagueSharp.Common;
using System.Linq;
using System;

namespace NechritoRiven.Classes.Extras
{
    class Logic : Core
    {
        public static AttackableUnit QTarget;
        public static bool forceQ;
        public static bool forceW;
        public static bool forceR;
        public static float LastQ;
        public static float LastR;

        public static void ForceW()
        {
            forceW = Spells.W.IsReady();
            Utility.DelayAction.Add(500, () => forceW = false);
        }
        public static void ForceQ(AttackableUnit target)
        {
            forceQ = true;
            QTarget = target;
        }
        public static void ForceSkill()
        {
            if (forceQ && QTarget != null && QTarget.IsValidTarget(Spells.E.Range + Player.BoundingRadius + 70) && Spells.Q.IsReady()) Spells.Q.Cast(QTarget.Position);

            if (forceW) Spells.W.Cast();
        }
        public static void OnCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            if (args.SData.Name.Contains("RivenTriCleave")) forceQ = false;
            if (args.SData.Name.Contains("RivenMartyr")) forceW = false;
            if (args.SData.Name == Spells.IsFirstR) forceR = false;
        }
        public static void Game_OnUpdate(EventArgs args)
        {
            if (MenuConfig.UseSkin)
            {
                Player.SetSkin(Player.CharData.BaseSkinName, MenuConfig.Menu.Item("Skin").GetValue<StringList>().SelectedIndex);
            }
            else Player.SetSkin(Player.CharData.BaseSkinName, Player.BaseSkinId);
        }
        public static Obj_AI_Base GetCenterMinion()
        {
            var minionposition = MinionManager.GetMinions(300 + Spells.Q.Range).Select(x => x.Position.To2D()).ToList();
            var center = MinionManager.GetBestCircularFarmLocation(minionposition, 250, 300 + Spells.Q.Range);

            return center.MinionsHit >= 3
                ? MinionManager.GetMinions(1000).OrderBy(x => x.Distance(center.Position)).FirstOrDefault()
                : null;
        }
    }
}
