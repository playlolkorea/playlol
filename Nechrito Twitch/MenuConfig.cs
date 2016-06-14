#region

using LeagueSharp.Common;

#endregion

namespace Nechrito_Twitch
{
    internal class MenuConfig
    {
        public static Menu Config;
        public static Menu TargetSelectorMenu;
        public static Orbwalking.Orbwalker Orbwalker;
        public static string MenuName = "Nechrito Twitch";

        public static void LoadMenu()
        {
            Config = new Menu(MenuName, MenuName, true);
            TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(TargetSelectorMenu);
            Config.AddSubMenu(TargetSelectorMenu);
            var orbwalker = new Menu("Orbwalker", "rorb");
            Orbwalker = new Orbwalking.Orbwalker(orbwalker);
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

            
            var misc = new Menu("Misc", "Misc");
            misc.AddItem(new MenuItem("QRecall", "QRecall").SetValue(new KeyBind('T', KeyBindType.Press)));
            Config.AddSubMenu(misc);

            var ExploitMenu = new Menu("ExploitMenu", "ExploitMenu");
            ExploitMenu.AddItem(new MenuItem("Exploit", "Exploits").SetValue(false).SetTooltip("Will Instant Q After Kill"));
            ExploitMenu.AddItem(new MenuItem("EAA", "E AA Q").SetValue(false).SetTooltip("Will cast E if killable by E + AA then Q"));
            Config.AddSubMenu(ExploitMenu);

            Config.AddToMainMenu();


        }
        // Menu Items
        public static bool StealEpic => Config.Item("StealEpic").GetValue<bool>();
        public static bool StealBuff => Config.Item("StealBuff").GetValue<bool>();
        public static bool UseW => Config.Item("UseW").GetValue<bool>();
        public static bool KsE => Config.Item("KsE").GetValue<bool>();
        public static bool LaneW => Config.Item("laneW").GetValue<bool>();
        public static bool HarassW => Config.Item("harassW").GetValue<bool>();
        public static bool Dind => Config.Item("dind").GetValue<bool>();
        public static bool Exploit => Config.Item("Exploit").GetValue<bool>();
        public static bool EAA => Config.Item("Exploit").GetValue<bool>();

        public static bool QRecall => Config.Item("QRecall").GetValue<KeyBind>().Active;

        public static int ESlider => Config.Item("ESlider").GetValue<Slider>().Value;
    }
}
