#region

using System;
using LeagueSharp.Common;
using NechritoRiven.Menus;

#endregion

namespace NechritoRiven.Event
{
    internal class Skinchanger : Core.Core
    {
        public static void Update(EventArgs args)
        {
            if (!MenuConfig.UseSkin)
            {
                Player.SetSkin(Player.CharData.BaseSkinName, Player.BaseSkinId);
                return;
            }
                Player.SetSkin(Player.CharData.BaseSkinName, MenuConfig.Config.Item("Skin").GetValue<StringList>().SelectedIndex);
        }
    }
}
