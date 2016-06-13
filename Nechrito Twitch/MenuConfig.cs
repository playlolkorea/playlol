using LeagueSharp;
using LeagueSharp.Common;

namespace Nechrito_Twitch
{
    class MenuConfig
    {
        public static Menu Config;
        public static Menu TargetSelectorMenu;
        public static Orbwalking.Orbwalker _orbwalker;
        public static string menuName = "Nechrito Twitch";

        public static void LoadMenu()
        {
            Config = new Menu(menuName, menuName, true);
            TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(TargetSelectorMenu);
            Config.AddSubMenu(TargetSelectorMenu);
            var orbwalker = new Menu("Orbwalker", "rorb");
            _orbwalker = new Orbwalking.Orbwalker(orbwalker);
            Config.AddSubMenu(orbwalker);

            var combo = new Menu("Combo", "Combo");

            combo.AddItem(new MenuItem("UseW", "Use W").SetValue(true));
            combo.AddItem(new MenuItem("KsE", "Ks E").SetValue(true));
            Config.AddSubMenu(combo);

            var harass = new Menu("Harass", "Harass");
            harass.AddItem(new MenuItem("harassW", "Use W").SetValue(true));
            harass.AddItem(new MenuItem("ESlider", "E Stack When Out Of AA Range").SetValue(new Slider(0, 0, 6)));
            Config.AddSubMenu(harass);

            var lane = new Menu("Lane", "Lane");
            lane.AddItem(new MenuItem("laneW", "Use W").SetValue(true));
            Config.AddSubMenu(lane);

            var steal = new Menu("Steal", "Steal");
            steal.AddItem(new MenuItem("StealEpic", "Dragon & Baron").SetValue(true));
            steal.AddItem(new MenuItem("StealBuff", "Buffs").SetValue(true));
            Config.AddSubMenu(steal);

            var draw = new Menu("Draw", "Draw");
            draw.AddItem(new MenuItem("dind", "Dmg Indicator").SetValue(true));
            Config.AddSubMenu(draw);

            /*
            var misc = new Menu("Misc", "Misc");
            misc.AddItem(new MenuItem("QRecall", "QRecall").SetValue(new KeyBind('B', KeyBindType.Press)));
            Config.AddSubMenu(misc);
            */
            Config.AddToMainMenu();


        }
        // Menu Items
        public static bool StealEpic => Config.Item("StealEpic").GetValue<bool>();
        public static bool StealBuff => Config.Item("StealBuff").GetValue<bool>();
        public static bool UseW => Config.Item("UseW").GetValue<bool>();
        public static bool KsE => Config.Item("KsE").GetValue<bool>();
        public static bool laneW => Config.Item("laneW").GetValue<bool>();
        public static bool harassW => Config.Item("harassW").GetValue<bool>();
        public static bool dind => Config.Item("dind").GetValue<bool>();
        public static bool QRecall => Config.Item("QRecall").GetValue<KeyBind>().Active;
        public static int ESlider => Config.Item("ESlider").GetValue<Slider>().Value;
    }
}
