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

            TargetSelectorMenu = new Menu("카톡 PLAYLOLKOREA", "카톡 PLAYLOLKOREA");
            TargetSelector.AddToMenu(TargetSelectorMenu);
            Config.AddSubMenu(TargetSelectorMenu);

            var orbwalker = new Menu("키설정", "rorb");
            Orbwalker = new Orbwalking.Orbwalker(orbwalker);
            Config.AddSubMenu(orbwalker);

            var animation = new Menu("움직임", "움직임");
            animation.AddItem(new MenuItem("QD", "Q1, Q2 딜레이").SetValue(new Slider(23, 23, 43)));
            animation.AddItem(new MenuItem("QLD", "Q3 딜레이").SetValue(new Slider(36, 36, 53)));
            animation.AddItem(new MenuItem("Qstrange", "x").SetValue(false)).SetTooltip("Enables Emote");
            animation.AddItem(new MenuItem("animLaugh", "웃기 애니메이션").SetValue(false));
            animation.AddItem(new MenuItem("animTaunt", "놀리기 애니메이션").SetValue(false));
            animation.AddItem(new MenuItem("animTalk", "농담 애니메이션").SetValue(false));
            animation.AddItem(new MenuItem("animDance", "춤 애니메이션").SetValue(false));
            Config.AddSubMenu(animation);

            var combo = new Menu("콤보(Spacebar)", "콤보(Spacebar)");
            combo.AddItem(new MenuItem("ignite", "자동 점화").SetValue(true)).SetTooltip("점화를 자동으로 막타에 사용한다.");
            combo.AddItem(new MenuItem("OverKillCheck", "궁 풀데미지").SetValue(true)).SetTooltip("데미지가 최고치일때 궁을 사용한다.");
            combo.AddItem(new MenuItem("AlwaysR", "선궁 사용").SetValue(new KeyBind('G', KeyBindType.Toggle))).SetTooltip("콤보 사용시 궁을 먼저 발동한다.");
            combo.AddItem(new MenuItem("AlwaysF", "선플래시 사용").SetValue(new KeyBind('L', KeyBindType.Toggle))).SetTooltip("콤보 사용시 플래시를 먼저 발동한다.");
            Config.AddSubMenu(combo);

            var lane = new Menu("라인클리어(V)", "라인클리어(V)");
            lane.AddItem(new MenuItem("LaneQ", "Q 사용").SetValue(true));
            lane.AddItem(new MenuItem("LaneW", "W 사용").SetValue(true));
            lane.AddItem(new MenuItem("LaneE", "E 사용").SetValue(true));
            Config.AddSubMenu(lane);

            var jngl = new Menu("정글클리어(V)", "정글클리어(V)");
            jngl.AddItem(new MenuItem("JungleQ", "Q 사용").SetValue(true));
            jngl.AddItem(new MenuItem("JungleW", "W 사용").SetValue(true));
            jngl.AddItem(new MenuItem("JungleE", "E 사용").SetValue(true));
            Config.AddSubMenu(jngl);

            var misc = new Menu("부가기능", "부가기능");
            misc.AddItem(new MenuItem("GapcloserMenu", "상대 돌진기에 자동W").SetValue(true));
            misc.AddItem(new MenuItem("InterruptMenu", "쉔궁,텔등에 자동W").SetValue(true));
            misc.AddItem(new MenuItem("KeepQ", "자동 Q유지").SetValue(true));
            Config.AddSubMenu(misc);

            var draw = new Menu("표시", "표시");
            draw.AddItem(new MenuItem("FleeSpot", "이동(Z)키 넘을벽 표시").SetValue(true));
            draw.AddItem(new MenuItem("Dind", "예상데미지 표시").SetValue(true));
            draw.AddItem(new MenuItem("DrawForceFlash", "선플래시 상태창 표시").SetValue(true));
            draw.AddItem(new MenuItem("DrawAlwaysR", "선궁 상태창 표시").SetValue(true));
            draw.AddItem(new MenuItem("DrawCB", "콤보(Spacebar)관련 표시").SetValue(false));
            draw.AddItem(new MenuItem("DrawBT", "버스트(T)관련 표시").SetValue(false));
            draw.AddItem(new MenuItem("DrawFH", "사용x").SetValue(false));
            draw.AddItem(new MenuItem("DrawHS", "견제(C)관련 표시").SetValue(false));
            Config.AddSubMenu(draw);

            var flee = new Menu("이동(Z)", "이동(Z)");
            flee.AddItem(new MenuItem("WallFlee", "이동시 벽넘기 지원").SetValue(true).SetTooltip("이동(Z)시 벽에다가가면 벽을 넘습니다."));
            flee.AddItem(new MenuItem("FleeYoumuu", "요우무 자동 발동").SetValue(true).SetTooltip("요우무를 자동으로 발동합니다."));
            Config.AddSubMenu(flee);

            var skin = new Menu("스킨바꾸기", "스킨바꾸기");
            skin.AddItem(new MenuItem("UseSkin", "스킨바꾸기").SetValue(false)).SetTooltip("스킨을 바꿀것인가?");
            skin.AddItem(new MenuItem("Skin", "스킨선택").SetValue(new StringList(new[] { "일반 리븐", "구원받은 리븐", "핏빛친위대 리븐", "전투토끼 리븐", "챔피언쉽 리븐", "화룡검 리븐", "아케이드 리븐" })));
            Config.AddSubMenu(skin);

            var qmove = new Menu("Q 무빙", "Q 무빙");
            qmove.AddItem(new MenuItem("QMove", "Q 무빙").SetValue(new KeyBind('K', KeyBindType.Press))).SetTooltip("마우스따라 Q 무빙");
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
