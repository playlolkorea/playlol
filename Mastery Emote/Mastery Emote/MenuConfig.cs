using LeagueSharp;
using LeagueSharp.Common;

namespace Mastery_Emote
{
    class MenuConfig
    {
        public static Menu Config;
        public static string menuName = "Mastery Emote";
        public static void LoadMenu()
        {
            Config = new Menu(menuName, menuName, true);
            var E = new Menu("Emote", "Emote");
            E.AddItem(new MenuItem("MasteryEmote", "Mastery").SetValue(true));
            E.AddItem(new MenuItem("Laugh", "Laugh").SetValue(true));
            E.AddItem(new MenuItem("Taunt", "Taunt").SetValue(false));
            E.AddItem(new MenuItem("Talk", "Joke").SetValue(false));
            E.AddItem(new MenuItem("Dance", "Dance").SetValue(false));
            Config.AddSubMenu(E);
            Config.AddToMainMenu();
        }
        public static bool Laugh => Config.Item("Laugh").GetValue<bool>();
        public static bool Taunt => Config.Item("Taunt").GetValue<bool>();
        public static bool Talk => Config.Item("Talk").GetValue<bool>();
        public static bool Dance => Config.Item("Dance").GetValue<bool>();
        public static bool MasteryEmote => Config.Item("MasteryEmote").GetValue<bool>();
    }
}
