using LeagueSharp.Common;

namespace NechritoRiven
{
    class MenuConfig : Core
    {
        public static Menu TargetSelectorMenu;
        private const string MenuName = "Nechrito Riven";
        public static Menu Menu { get; set; } = new Menu(MenuName, MenuName, true);
        public static void Load()
        {
            TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(TargetSelectorMenu);
            Menu.AddSubMenu(TargetSelectorMenu);

            var orbwalker = new Menu("Orbwalker", "rorb");
            Orb = new Orbwalking.Orbwalker(orbwalker);
            Menu.AddSubMenu(orbwalker);

            var animation = new Menu("Animation", "Animation");
            var emoteMenu = new Menu("Emote", "Emote");
            animation.AddItem(new MenuItem("qReset", "Fast & Legit Q").SetValue(true)).SetTooltip("Uses dance in animation, off will not dance & look more legit");
            emoteMenu.AddItem(new MenuItem("Qstrange", "Enable").SetValue(false)).SetTooltip("Enables Emote");
            emoteMenu.AddItem(new MenuItem("animLaugh", "Laugh").SetValue(false));
            emoteMenu.AddItem(new MenuItem("animTaunt", "Taunt").SetValue(false));
            emoteMenu.AddItem(new MenuItem("animTalk", "Joke").SetValue(false));
            emoteMenu.AddItem(new MenuItem("animDance", "Dance").SetValue(false));
            animation.AddSubMenu(emoteMenu);
            Menu.AddSubMenu(animation);

            /*
            var burst = new Menu("Burst", "burst");
            burst.AddItem(new MenuItem("OneShot", "OneShot Burst").SetValue(true)).SetTooltip("E R1 Tiamat AA W Q AA R2 Q");
            Menu.AddSubMenu(burst);
            */

            var combo = new Menu("Combo", "Combo");
            combo.AddItem(new MenuItem("ignite", "Auto Ignite").SetValue(true)).SetTooltip("Auto Ignite When target is killable");
            combo.AddItem(new MenuItem("AlwaysR", "Force R").SetValue(new KeyBind('G', KeyBindType.Toggle))).SetTooltip("Off will only use R when target is killable");
            combo.AddItem(new MenuItem("AlwaysF", "Force Flash").SetValue(new KeyBind('L', KeyBindType.Toggle))).SetTooltip("Off Will only use R when target is killable");
            Menu.AddSubMenu(combo);

            var lane = new Menu("Lane", "Lane");
            lane.AddItem(new MenuItem("FastC", "Fast Laneclear").SetValue(false)).SetTooltip("Will Q AA waveclear faster than usual");
            lane.AddItem(new MenuItem("LaneQ", "Use Q").SetValue(true));
            lane.AddItem(new MenuItem("LaneW", "Use W").SetValue(true));
            lane.AddItem(new MenuItem("LaneE", "Use E").SetValue(true));
            Menu.AddSubMenu(lane);

            var jngl = new Menu("Jungle", "Jungle");
            jngl.AddItem(new MenuItem("JungleQ", "Use Q").SetValue(true));
            jngl.AddItem(new MenuItem("JungleW", "Use W").SetValue(true));
            jngl.AddItem(new MenuItem("JungleE", "Use E").SetValue(true));
            Menu.AddSubMenu(jngl);

            var misc = new Menu("Misc", "Misc");
            misc.AddItem(new MenuItem("KeepQ", "Keep Q Alive").SetValue(true));
            misc.AddItem(new MenuItem("QD", "Q1, Q2 Delay").SetValue(new Slider(29, 23, 43)));
            misc.AddItem(new MenuItem("QLD", "Q3 Delay").SetValue(new Slider(39, 36, 53)));
            Menu.AddSubMenu(misc);

            var draw = new Menu("Draw", "Draw");
            draw.AddItem(new MenuItem("FleeSpot", "Draw Flee Spots").SetValue(true));
            draw.AddItem(new MenuItem("Dind", "Damage Indicator").SetValue(true));
            draw.AddItem(new MenuItem("DrawForceFlash", "Flash Status").SetValue(true));
            draw.AddItem(new MenuItem("DrawAlwaysR", "R Status").SetValue(true));
            draw.AddItem(new MenuItem("DrawTimer1", "Draw Q Expiry Time").SetValue(false));
            draw.AddItem(new MenuItem("DrawTimer2", "Draw R Expiry Time").SetValue(false));
            draw.AddItem(new MenuItem("DrawCB", "Combo Engage").SetValue(false));
            draw.AddItem(new MenuItem("DrawBT", "Burst Engage").SetValue(false));
            draw.AddItem(new MenuItem("DrawFH", "FastHarass Engage").SetValue(false));
            draw.AddItem(new MenuItem("DrawHS", "Harass Engage").SetValue(false));
            Menu.AddSubMenu(draw);

            var flee = new Menu("Flee", "Flee");
            flee.AddItem(new MenuItem("WallFlee", "WallJump in Flee").SetValue(true).SetTooltip("Jumps over walls in flee mode"));
            Menu.AddSubMenu(flee);

            var qmove = new Menu("qmove", "Q Move");
            qmove.AddItem(new MenuItem("QMove", "Q Move").SetValue(new KeyBind('K', KeyBindType.Press))).SetTooltip("Will Q Move to mouse");
            Menu.AddSubMenu(qmove);

            var skin = new Menu("SkinChanger", "SkinChanger");
            skin.AddItem(new MenuItem("UseSkin", "Use SkinChanger").SetValue(false)).SetTooltip("Toggles Skinchanger");
            skin.AddItem(new MenuItem("Skin", "Skin").SetValue(new StringList(new[] { "Default", "Redeemed", "Crimson Elite", "Battle Bunny", "Championship", "Dragonblade", "Arcade" })));
            Menu.AddSubMenu(skin);

            var credit = new Menu("Credit", "Credit");
            credit.AddItem(new MenuItem("nechrito", "Originally made by Hoola, completely re-written By Nechrito"));
            Menu.AddSubMenu(credit);

            Menu.AddToMainMenu();
        }
        public static bool FastC => Menu.Item("FastC").GetValue<bool>();
        public static bool FleeSpot => Menu.Item("FleeSpot").GetValue<bool>();
        public static bool WallFlee => Menu.Item("WallFlee").GetValue<bool>();
        public static bool jnglQ => Menu.Item("JungleQ").GetValue<bool>();
        public static bool jnglW => Menu.Item("JungleW").GetValue<bool>();
        public static bool jnglE => Menu.Item("JungleE").GetValue<bool>();
        public static bool UseSkin => Menu.Item("UseSkin").GetValue<bool>();
        public static bool AlwaysF => Menu.Item("AlwaysF").GetValue<KeyBind>().Active;
        public static bool QMove => Menu.Item("QMove").GetValue<KeyBind>().Active;
        public static bool ignite => Menu.Item("ignite").GetValue<bool>();
        public static bool ForceFlash => Menu.Item("DrawForceFlash").GetValue<bool>();
        public static bool IreliaLogic=> Menu.Item("IreliaLogic").GetValue<bool>();
        public static bool QReset => Menu.Item("qReset").GetValue<bool>();
        public static bool Dind => Menu.Item("Dind").GetValue<bool>();
        public static bool DrawCb => Menu.Item("DrawCB").GetValue<bool>();
        public static bool AnimLaugh => Menu.Item("animLaugh").GetValue<bool>();
        public static bool AnimTaunt => Menu.Item("animTaunt").GetValue<bool>();
        public static bool AnimDance => Menu.Item("animDance").GetValue<bool>();
        public static bool AnimTalk => Menu.Item("animTalk").GetValue<bool>();
        public static bool DrawAlwaysR => Menu.Item("DrawAlwaysR").GetValue<bool>();
        public static bool KeepQ => Menu.Item("KeepQ").GetValue<bool>();
        public static bool DrawFh => Menu.Item("DrawFH").GetValue<bool>();
        public static bool DrawTimer1 => Menu.Item("DrawTimer1").GetValue<bool>();
        public static bool DrawTimer2 => Menu.Item("DrawTimer2").GetValue<bool>();
        public static bool DrawHs => Menu.Item("DrawHS").GetValue<bool>();
        public static bool DrawBt => Menu.Item("DrawBT").GetValue<bool>();
        public static bool AlwaysR => Menu.Item("AlwaysR").GetValue<KeyBind>().Active;
        public static int Qd => Menu.Item("QD").GetValue<Slider>().Value;
        public static int Qld => Menu.Item("QLD").GetValue<Slider>().Value;
        public static bool LaneW => Menu.Item("LaneW").GetValue<bool>();
        public static bool LaneE => Menu.Item("LaneE").GetValue<bool>();
        public static bool Qstrange => Menu.Item("Qstrange").GetValue<bool>();
        public static bool LaneQ => Menu.Item("LaneQ").GetValue<bool>();



    }
}