#region

using LeagueSharp.Common;

#endregion

namespace NechritoRiven.Menus
{
    internal class MenuConfig : Core.Core
    {
        public static Menu Config;
        public static Menu TargetSelectorMenu;
        public static string MenuName = "리븐 설정";

        public static void LoadMenu()
        {
            Config = new Menu(MenuName, MenuName, true);

            TargetSelectorMenu = new Menu("translate by플레이롤", "translate by플레이롤");
            TargetSelector.AddToMenu(TargetSelectorMenu);
            Config.AddSubMenu(TargetSelectorMenu);

            var orbwalker = new Menu("키설정", "rorb");
            Orbwalker = new Orbwalking.Orbwalker(orbwalker);
            Config.AddSubMenu(orbwalker);

            var animation = new Menu("움직임", "움직임");
            animation.AddItem(new MenuItem("QD", "Q1, Q2 딜레이").SetValue(new Slider(23, 23, 43)));
            animation.AddItem(new MenuItem("QLD", "Q3 딜레이").SetValue(new Slider(36, 36, 53)));
            animation.AddItem(new MenuItem("Qstrange", "사용x").SetValue(false)).SetTooltip("Enables Emote");
            animation.AddItem(new MenuItem("animLaugh", "웃기 애니메이션").SetValue(false));
            animation.AddItem(new MenuItem("animTaunt", "놀리기 애니메이션").SetValue(false));
            animation.AddItem(new MenuItem("animTalk", "농담 애니메이션").SetValue(false));
            animation.AddItem(new MenuItem("animDance", "춤 애니메이션").SetValue(false));
            Config.AddSubMenu(animation);

            var combo = new Menu("콤보(Spacebar)", "콤보(spacebar)");
            combo.AddItem(new MenuItem("ignite", "자동 점화").SetValue(true)).SetTooltip("Auto Ignite When target is killable");
            combo.AddItem(new MenuItem("OverKillCheck", "R Max Damage").SetValue(true)).SetTooltip("Will save R for max damage");
            combo.AddItem(new MenuItem("AlwaysR", "Force R").SetValue(new KeyBind('G', KeyBindType.Toggle))).SetTooltip("Off will only use R when target is killable");
            combo.AddItem(new MenuItem("AlwaysF", "Force Flash").SetValue(new KeyBind('L', KeyBindType.Toggle))).SetTooltip("Off Will only use Flash when target is killable");
            Config.AddSubMenu(combo);

            var lane = new Menu("Lane", "Lane");
            lane.AddItem(new MenuItem("LaneQ", "Use Q").SetValue(true));
            lane.AddItem(new MenuItem("LaneW", "Use W").SetValue(true));
            lane.AddItem(new MenuItem("LaneE", "Use E").SetValue(true));
            Config.AddSubMenu(lane);

            var jngl = new Menu("Jungle", "Jungle");
            jngl.AddItem(new MenuItem("JungleQ", "Use Q").SetValue(true));
            jngl.AddItem(new MenuItem("JungleW", "Use W").SetValue(true));
            jngl.AddItem(new MenuItem("JungleE", "Use E").SetValue(true));
            Config.AddSubMenu(jngl);

            var misc = new Menu("Misc", "Misc");
            misc.AddItem(new MenuItem("GapcloserMenu", "Anti-Gapcloser").SetValue(true));
            misc.AddItem(new MenuItem("InterruptMenu", "Interrupter").SetValue(true));
            misc.AddItem(new MenuItem("KeepQ", "Keep Q Alive").SetValue(true));
            Config.AddSubMenu(misc);

            var draw = new Menu("Draw", "Draw");
            draw.AddItem(new MenuItem("FleeSpot", "Draw Flee Spots").SetValue(true));
            draw.AddItem(new MenuItem("Dind", "Damage Indicator").SetValue(true));
            draw.AddItem(new MenuItem("DrawForceFlash", "Flash Status").SetValue(true));
            draw.AddItem(new MenuItem("DrawAlwaysR", "R Status").SetValue(true));
            draw.AddItem(new MenuItem("DrawCB", "Combo Engage").SetValue(false));
            draw.AddItem(new MenuItem("DrawBT", "Burst Engage").SetValue(false));
            draw.AddItem(new MenuItem("DrawFH", "FastHarass Engage").SetValue(false));
            draw.AddItem(new MenuItem("DrawHS", "Harass Engage").SetValue(false));
            Config.AddSubMenu(draw);

            var flee = new Menu("Flee", "Flee");
            flee.AddItem(new MenuItem("WallFlee", "WallJump in Flee").SetValue(true).SetTooltip("Jumps over walls in flee mode"));
            flee.AddItem(new MenuItem("FleeYoumuu", "Youmuu's Ghostblade").SetValue(true).SetTooltip("Will flee with ghostblade"));
            Config.AddSubMenu(flee);

            var skin = new Menu("SkinChanger", "SkinChanger");
            skin.AddItem(new MenuItem("UseSkin", "Use SkinChanger").SetValue(false)).SetTooltip("Toggles Skinchanger");
            skin.AddItem(new MenuItem("Skin", "Skin").SetValue(new StringList(new[] { "Default", "Redeemed", "Crimson Elite", "Battle Bunny", "Championship", "Dragonblade", "Arcade" })));
            Config.AddSubMenu(skin);

            var qmove = new Menu("Q Move", "Q Move");
            qmove.AddItem(new MenuItem("QMove", "Q Move").SetValue(new KeyBind('K', KeyBindType.Press))).SetTooltip("Will Q Move to mouse");
            Config.AddSubMenu(qmove);

            Config.AddToMainMenu();
        }

        public static bool GapcloserMenu => Config.Item("GapcloserMenu").GetValue<bool>();
        public static bool InterruptMenu => Config.Item("InterruptMenu").GetValue<bool>();

        public static bool QMove => Config.Item("QMove").GetValue<KeyBind>().Active;

        public static StringList ItemList => Config.Item("ItemList").GetValue<StringList>();

        public static int Qd => Config.Item("QD").GetValue<Slider>().Value;
        public static int Qld => Config.Item("QLD").GetValue<Slider>().Value;

        public static bool FleeYomuu => Config.Item("FleeYoumuu").GetValue<bool>();
        public static bool OverKillCheck => Config.Item("OverKillCheck").GetValue<bool>();
        public static bool FleeSpot => Config.Item("FleeSpot").GetValue<bool>();
        public static bool WallFlee => Config.Item("WallFlee").GetValue<bool>();
        public static bool JnglQ => Config.Item("JungleQ").GetValue<bool>();
        public static bool JnglW => Config.Item("JungleW").GetValue<bool>();
        public static bool JnglE => Config.Item("JungleE").GetValue<bool>();
        public static bool UseSkin => Config.Item("UseSkin").GetValue<bool>();
        public static bool AlwaysF => Config.Item("AlwaysF").GetValue<KeyBind>().Active;
        public static bool Ignite => Config.Item("ignite").GetValue<bool>();
        public static bool ForceFlash => Config.Item("DrawForceFlash").GetValue<bool>();
        public static bool IreliaLogic => Config.Item("IreliaLogic").GetValue<bool>();
        public static bool QReset => Config.Item("qReset").GetValue<bool>();
        public static bool Dind => Config.Item("Dind").GetValue<bool>();
        public static bool DrawCb => Config.Item("DrawCB").GetValue<bool>();
        public static bool AnimLaugh => Config.Item("animLaugh").GetValue<bool>();
        public static bool AnimTaunt => Config.Item("animTaunt").GetValue<bool>();
        public static bool AnimDance => Config.Item("animDance").GetValue<bool>();
        public static bool AnimTalk => Config.Item("animTalk").GetValue<bool>();
        public static bool DrawAlwaysR => Config.Item("DrawAlwaysR").GetValue<bool>();
        public static bool KeepQ => Config.Item("KeepQ").GetValue<bool>();
        public static bool DrawFh => Config.Item("DrawFH").GetValue<bool>();
        public static bool DrawHs => Config.Item("DrawHS").GetValue<bool>();
        public static bool DrawBt => Config.Item("DrawBT").GetValue<bool>();
        public static bool AlwaysR => Config.Item("AlwaysR").GetValue<KeyBind>().Active;
        public static int WallWidth => Config.Item("WallWidth").GetValue<Slider>().Value;
        public static bool LaneW => Config.Item("LaneW").GetValue<bool>();
        public static bool LaneE => Config.Item("LaneE").GetValue<bool>();
        public static bool Qstrange => Config.Item("Qstrange").GetValue<bool>();
        public static bool LaneQ => Config.Item("LaneQ").GetValue<bool>();
    }
}
