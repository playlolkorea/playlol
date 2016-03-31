using LeagueSharp;
using LeagueSharp.Common;
using ItemData = LeagueSharp.Common.Data.ItemData;



namespace Nechrito_Rengar
{
    class Logic
    {
        public static bool HasTitan() => Items.HasItem(3748) && Items.CanUseItem(3748);

        public static void CastTitan()
        {
            if (Items.HasItem(3748) && Items.CanUseItem(3748))
            {
                Items.UseItem(3748);
            }
        }
        public static void CastYoumoo()
        {
            if (ItemData.Youmuus_Ghostblade.GetItem().IsReady()) ItemData.Youmuus_Ghostblade.GetItem().Cast();
        }
        public static bool _forceItem;
        public static int Item
           =>
               Items.CanUseItem(3077) && Items.HasItem(3077)
                   ? 3077
                   : Items.CanUseItem(3074) && Items.HasItem(3074) ? 3074 : 0;
        public static bool HasItem()
            => ItemData.Tiamat_Melee_Only.GetItem().IsReady() || ItemData.Ravenous_Hydra_Melee_Only.GetItem().IsReady();
        public static void ForceItem()
        {
            if (Items.CanUseItem(Item) && Items.HasItem(Item) && Item != 0) _forceItem = true;
            Utility.DelayAction.Add(500, () => _forceItem = false);
        }
    }
}
