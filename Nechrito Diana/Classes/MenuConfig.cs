using LeagueSharp.Common;

namespace Nechrito_Diana
{
    class MenuConfig
    {
        public static Menu Config;
        public static Menu TargetSelectorMenu;
        public static Orbwalking.Orbwalker _orbwalker;
        public static string menuName = "Nechrito Diana";
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
            combo.AddItem(new MenuItem("Misaya", "Force Misaya").SetValue(false)).SetTooltip("Else Auto Misaya Combo if target is killable");
            Config.AddSubMenu(combo);
            
            var lane = new Menu("Lane", "Lane");
            lane.AddItem(new MenuItem("LaneQ", "Use Q").SetValue(false)).SetTooltip("Uses Q in laneclear");
            lane.AddItem(new MenuItem("LaneW", "Use W").SetValue(true)).SetTooltip("Uses W in laneclear");
            Config.AddSubMenu(lane);

            var jungle = new Menu("Jungle", "Jungle");
            jungle.AddItem(new MenuItem("jnglQR", "QR Gapclose").SetValue(true)).SetTooltip("At 700~ units will throw QR to mob position");
            jungle.AddItem(new MenuItem("jnglE", "E Interrupt").SetValue(new Slider(15, 0))).SetTooltip("Interrupt Mob With E");
            jungle.AddItem(new MenuItem("jnglW", "Use W").SetValue(true)).SetTooltip("Use W and passive in Jungle");
            Config.AddSubMenu(jungle);

            var killsteal = new Menu("Killsteal", "Killsteal");
            killsteal.AddItem(new MenuItem("ksQ", "Killsteal Q").SetValue(true)).SetTooltip("Uses Q on a killable Target");
            killsteal.AddItem(new MenuItem("ksR", "Killsteal R").SetValue(true)).SetTooltip("Try to QR target or just R");
            killsteal.AddItem(new MenuItem("ignite", "Auto Ignite").SetValue(true)).SetTooltip("Auto Ignite When target is killable");
            killsteal.AddItem(new MenuItem("ksSmite", "Auto Smite").SetValue(true)).SetTooltip("Auto Smite When target is killable");
            Config.AddSubMenu(killsteal);

            var misc = new Menu("Misc", "Misc");
            misc.AddItem(new MenuItem("AutoW", "Shield").SetValue(new Slider(15, 0))).SetTooltip("Shields With W");
            misc.AddItem(new MenuItem("Gapcloser", "Gapcloser").SetValue(true)).SetTooltip("Gapclose with E");
            misc.AddItem(new MenuItem("Interrupt", "Interrupt").SetValue(true)).SetTooltip("Interrupt with E");
            misc.AddItem(new MenuItem("AutoSmite", "Auto Smite").SetValue(true)).SetTooltip("Auto Smite target");
            Config.AddSubMenu(misc);

            var skin = new Menu("SkinChanger", "SkinChanger");
            skin.AddItem(new MenuItem("UseSkin", "Use SkinChanger").SetValue(false)).SetTooltip("Toggles Skinchanger");
            skin.AddItem(new MenuItem("Skin", "Skin").SetValue(new StringList(new[]{ "Default", "Dark Valkyrie Diana", "Lunar Goddess Diana", "Infernal Diana"})));
            Config.AddSubMenu(skin);

            var flee = new Menu("Flee", "Flee");
            flee.AddItem(new MenuItem("FleeMouse", "Flee").SetValue(new KeyBind('A', KeyBindType.Press))).SetTooltip("Will Flee To Mouse");
            Config.AddSubMenu(flee);

            SPrediction.Prediction.Initialize(Config);
            Config.AddToMainMenu();
        }
        public static bool Misaya => Config.Item("Misaya").GetValue<bool>();
        public static bool LaneQ => Config.Item("LaneQ").GetValue<bool>();
        public static bool LaneW => Config.Item("LaneW").GetValue<bool>();
        public static bool ksSmite => Config.Item("ksSmite").GetValue<bool>();
        public static bool ksQ => Config.Item("ksQ").GetValue<bool>();
        public static bool ksR => Config.Item("ksR").GetValue<bool>();
        public static bool Interrupt => Config.Item("Interrupt").GetValue<bool>();
        public static bool Gapcloser => Config.Item("Gapcloser").GetValue<bool>();
        public static bool jnglW => Config.Item("jnglW").GetValue<bool>();
        public static bool jnglQR => Config.Item("jnglQR").GetValue<bool>();
        public static bool ignite => Config.Item("ignite").GetValue<bool>();
        public static bool AutoSmite => Config.Item("AutoSmite").GetValue<bool>();
        public static bool UseSkin => Config.Item("UseSkin").GetValue<bool>();
        public static bool FleeMouse => Config.Item("FleeMouse").GetValue<KeyBind>().Active;
        public static Slider AutoW => Config.Item("AutoW").GetValue<Slider>();
        public static Slider jnglE => Config.Item("jnglE").GetValue<Slider>();
    }
}
