using ItemData = LeagueSharp.Common.Data.ItemData;
using LeagueSharp;
using LeagueSharp.Common;

namespace Nechrito_Rengar
{
    class Logic
    {   
        public static void CastHydra()
        {
            if (ItemData.Ravenous_Hydra_Melee_Only.GetItem().IsReady())
                ItemData.Ravenous_Hydra_Melee_Only.GetItem().Cast();
            else if (ItemData.Tiamat_Melee_Only.GetItem().IsReady())
                ItemData.Tiamat_Melee_Only.GetItem().Cast();
        }
        public static void CastYoumoo()
        {
            if (ItemData.Youmuus_Ghostblade.GetItem().IsReady()) ItemData.Youmuus_Ghostblade.GetItem().Cast();
        }
        public static bool HasItem() => ItemData.Tiamat_Melee_Only.GetItem().IsReady() || ItemData.Ravenous_Hydra_Melee_Only.GetItem().IsReady();
        public static void SmiteLogic()
        {
            var target = TargetSelector.GetTarget(300f, TargetSelector.DamageType.True);
            if (ItemData.Stalkers_Blade.GetItem().IsReady()) ItemData.Stalkers_Blade.GetItem().Cast(target);
        }
    }
}
