using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using NechritoRiven.Menus;

namespace NechritoRiven.Event
{
    class Trinkets : Core.Core
    {
        public static void Update(EventArgs args)
        {
            if (!MenuConfig.Buytrinket || Player.Level < 9 || !Player.InShop() || Items.HasItem(3363) || Items.HasItem(3364)) return;

            switch (MenuConfig.Trinketlist.SelectedIndex)
            {
                case 0:
                    Player.BuyItem(ItemId.Oracles_Lens_Trinket);
                    break;
                case 1:
                    Player.BuyItem(ItemId.Farsight_Orb_Trinket);
                    break;
            }
        }
    }
}
