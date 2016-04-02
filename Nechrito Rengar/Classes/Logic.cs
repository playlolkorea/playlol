using LeagueSharp;
using LeagueSharp.Common;
using System.Linq;
using ItemData = LeagueSharp.Common.Data.ItemData;

namespace Nechrito_Rengar
{
    class Logic
    {
        public static Obj_AI_Hero Player => ObjectManager.Player;
        private static readonly int[] BlueSmite = { 3706, 1400, 1401, 1402, 1403 };

        private static readonly int[] RedSmite = { 3715, 1415, 1414, 1413, 1412 };
        protected static SpellSlot Smite;
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
        protected static void SmiteCombo()
        {
            if (BlueSmite.Any(id => Items.HasItem(id)))
            {
                Smite = Player.GetSpellSlot("s5_summonersmiteplayerganker");
                return;
            }

            if (RedSmite.Any(id => Items.HasItem(id)))
            {
                Smite = Player.GetSpellSlot("s5_summonersmiteduel");
                return;
            }

            Smite = Player.GetSpellSlot("summonersmite");
        }



    }
}
