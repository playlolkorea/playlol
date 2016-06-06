using LeagueSharp;
using LeagueSharp.Common;
using NechritoRiven.Core;
using NechritoRiven.Menus;

namespace NechritoRiven.Event
{
    class Anim : Core.Core
    {
        public static void OnPlay(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe) return;

            switch (args.Animation)
            {
                case "Spell1a":
                    lastQ = Utils.GameTimeTickCount;
                    if (MenuConfig.Qstrange && _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None) Game.Say("/d");
                    Qstack = 2;
                    if (_orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None &&
                        _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LastHit &&
                        _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Flee)
                        Utility.DelayAction.Add(MenuConfig.Qd * 10 + 1, Reset);
                    break;
                case "Spell1b":
                    lastQ = Utils.GameTimeTickCount;
                    if (MenuConfig.Qstrange && _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None) Game.Say("/d");
                    Qstack = 3;
                    if (_orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None &&
                        _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LastHit &&
                        _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Flee)
                        Utility.DelayAction.Add(MenuConfig.Qd * 10 + 1, Reset);
                    break;
                case "Spell1c":
                    lastQ = Utils.GameTimeTickCount;
                    if (MenuConfig.Qstrange && _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None) Game.Say("/d");
                    Qstack = 1;
                    if (_orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None &&
                        _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LastHit &&
                        _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Flee)
                        Utility.DelayAction.Add(MenuConfig.Qld * 10 + 3, Reset);
                    break;
                case "Spell3":
                    if ((_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Burst ||
                         _orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo ||
                         _orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.FastHarass ||
                         _orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Flee)) Usables.CastYoumoo();
                    break;
                case "Spell4a":
                    lastR = Utils.GameTimeTickCount;
                    break;
                case "Spell4b":
                    var target = TargetSelector.GetSelectedTarget();
                    if (Spells.Q.IsReady() && target.IsValidTarget()) ForceCastQ(target);
                    break;
            }
        }
        private static void Reset()
        {
            Orbwalking.LastAATick = 0;
            if (MenuConfig.QReset) Game.Say("/d");
            Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position.Extend(Game.CursorPos, Player.Distance(Game.CursorPos) + 10));
            Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position - 200);
        }
    }
}
