using LeagueSharp;
using LeagueSharp.Common;
using ItemData = LeagueSharp.Common.Data.ItemData;
using System.Linq;
using System;

namespace NechritoRiven
{
    class Logic : Riven
    {
        public static void Game_OnUpdate(EventArgs args)
        {
            if (MenuConfig.UseSkin)
            {
                Player.SetSkin(Player.CharData.BaseSkinName, MenuConfig.Config.Item("Skin").GetValue<StringList>().SelectedIndex);
            }
            else Player.SetSkin(Player.CharData.BaseSkinName, Player.BaseSkinId);
        }
        public static bool _forceQ;
        public static bool _forceW;
        public static bool _forceR;
        public static bool _forceR2;
        public static bool _forceItem;
        public static float _lastQ;
        public static float _lastR;
        public static AttackableUnit _qtarget;
        public static int WRange => Player.HasBuff("RivenFengShuiEngine")
            ? 330
            : 265;

        public static bool InWRange(AttackableUnit t) => t != null && t.IsValidTarget(WRange);
        public static bool InQRange(GameObject target)
        {
            return target != null && (Player.HasBuff("RivenFengShuiEngine")
                ? 330 >= Player.Distance(target.Position)
                : 265 >= Player.Distance(target.Position));
        }

        public static Obj_AI_Base GetCenterMinion()
        {
            var minionposition = MinionManager.GetMinions(300 + Spells._q.Range).Select(x => x.Position.To2D()).ToList();
            var center = MinionManager.GetBestCircularFarmLocation(minionposition, 250, 300 + Spells._q.Range);

            return center.MinionsHit >= 3
                ? MinionManager.GetMinions(1000).OrderBy(x => x.Distance(center.Position)).FirstOrDefault()
                : null;
        }
        public static void ForceSkill()
        {
            if (_forceQ && _qtarget != null && _qtarget.IsValidTarget(Spells._e.Range + Player.BoundingRadius + 70) &&
                Spells._q.IsReady())
                Spells._q.Cast(_qtarget.Position);
            if (_forceW) Spells._w.Cast();
            if (_forceR && Spells._r.Instance.Name == IsFirstR) Spells._r.Cast();
            if (_forceR2 && Spells._r.Instance.Name == IsSecondR)
            {
                var target = TargetSelector.GetSelectedTarget();
                if (target != null) Spells._r.Cast(target.Position);
            }
        }

        public static void ForceW()
        {
            _forceW = Spells._w.IsReady();
            Utility.DelayAction.Add(500, () => _forceW = false);
        }
        public static void FlashW()
        {
            var target = TargetSelector.GetSelectedTarget();
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                Spells._w.Cast();
                Utility.DelayAction.Add(10, () => Player.Spellbook.CastSpell(Spells.Flash, target.Position));
            }
        }
        public static void ForceCastQ(AttackableUnit target)
        {
            _forceQ = true;
            _qtarget = target;
        }

        public static void CastHydra()
        {
            if (ItemData.Ravenous_Hydra_Melee_Only.GetItem().IsReady())
                ItemData.Ravenous_Hydra_Melee_Only.GetItem().Cast();
            if (ItemData.Tiamat_Melee_Only.GetItem().IsReady())
                ItemData.Tiamat_Melee_Only.GetItem().Cast();
        }

        public static void CastYoumoo()
        {
            if (ItemData.Youmuus_Ghostblade.GetItem().IsReady()) ItemData.Youmuus_Ghostblade.GetItem().Cast();
        }

        public static void OnCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            if (args.SData.Name.Contains("ItemTiamatCleave")) _forceItem = false;
            if (args.SData.Name.Contains("RivenTriCleave")) _forceQ = false;
            if (args.SData.Name.Contains("RivenMartyr")) _forceW = false;
            if (args.SData.Name == IsFirstR) _forceR = false;
            if (args.SData.Name == IsSecondR) _forceR2 = false;
        }
    }
}