using LeagueSharp;
using LeagueSharp.Common;
using ItemData = LeagueSharp.Common.Data.ItemData;



namespace Nechrito_Rengar
{
    class Logic
    {
        public static bool forceItem;
       

        
        public static void CastHydra()
        {
            if (ItemData.Ravenous_Hydra_Melee_Only.GetItem().IsReady()) ItemData.Ravenous_Hydra_Melee_Only.GetItem().Cast();
        }
        public static void CastYoumoo()
        {
            if (ItemData.Youmuus_Ghostblade.GetItem().IsReady()) ItemData.Youmuus_Ghostblade.GetItem().Cast();
        }
        public static bool HasItem() => ItemData.Tiamat_Melee_Only.GetItem().IsReady() || ItemData.Ravenous_Hydra_Melee_Only.GetItem().IsReady();
    }
}
