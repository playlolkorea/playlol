using LeagueSharp.Common;

namespace Nechrito_Rengar
{
    class MenuConfig
    {
        public static Menu Config;
        public static Menu TargetSelectorMenu;
        public static Orbwalking.Orbwalker _orbwalker;

        public static string menuName = "Nechrito Rengar";
        public static void LoadMenu()
        {
            Config = new Menu(menuName, menuName, true);

            TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(TargetSelectorMenu);
            Config.AddSubMenu(TargetSelectorMenu);
            var orbwalker = new Menu("Orbwalker", "rorb");
            _orbwalker = new Orbwalking.Orbwalker(orbwalker);
            Config.AddSubMenu(orbwalker);


            var misc = new Menu("Misc", "Misc");
            misc.AddItem(new MenuItem("Passive", "Save Passive")).SetValue(new KeyBind('G', KeyBindType.Toggle));
            Config.AddSubMenu(misc);


            var draw = new Menu("Draw", "Draw");
            draw.AddItem(new MenuItem("dind", "Damage Indicator")).SetValue(true);
            Config.AddSubMenu(draw);

            Config.AddToMainMenu();
        }
        public static bool Passive => Config.Item("Passive").GetValue<KeyBind>().Active;
        public static bool dind => Config.Item("dind").GetValue<bool>();
        public static bool QAA => Config.Item("QAA").GetValue<bool>();
        public static bool GankCombo => Config.Item("GankCombo").GetValue<bool>();
        public static bool OneShot => Config.Item("OneShot").GetValue<bool>();

    }
}
