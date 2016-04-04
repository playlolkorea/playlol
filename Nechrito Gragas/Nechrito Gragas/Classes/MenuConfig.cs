using LeagueSharp.Common;

namespace Nechrito_Gragas
{
    class MenuConfig
    {
        public static Menu Config;
        public static Menu TargetSelectorMenu;
        public static Orbwalking.Orbwalker _orbwalker;

        public static string menuName = "Nechrito Gragas";
        public static void LoadMenu()
        {
            Config = new Menu(menuName, menuName, true);

            TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(TargetSelectorMenu);
            Config.AddSubMenu(TargetSelectorMenu);
            var orbwalker = new Menu("Orbwalker", "rorb");
            _orbwalker = new Orbwalking.Orbwalker(orbwalker);
            Config.AddSubMenu(orbwalker);

            //COMBOS ETC HERE
            var draw = new Menu("Draw", "Draw");
            draw.AddItem(new MenuItem("dind", "Damage Indicator")).SetValue(true);
            Config.AddSubMenu(draw);

            Config.AddToMainMenu();
        }
       public static bool dind => Config.Item("dind").GetValue<bool>();
    }
}
