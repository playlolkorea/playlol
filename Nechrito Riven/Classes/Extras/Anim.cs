using LeagueSharp;
using LeagueSharp.Common;
using System.Linq;
using System;


namespace NechritoRiven.Classes.Extras
{
    class Anim : Core
    {
        public static void OnPlay(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe)
            { return; }

            switch (args.Animation)
            {
                case "Spell1a":
                    Logic.LastQ = Utils.GameTimeTickCount;
                    if (MenuConfig.Qstrange && Orb.ActiveMode != Orbwalking.OrbwalkingMode.None) Game.Say("/d");
                    Qstack = 2;
                    if (Orb.ActiveMode != Orbwalking.OrbwalkingMode.None && Orb.ActiveMode != Orbwalking.OrbwalkingMode.LastHit && Orb.ActiveMode != Orbwalking.OrbwalkingMode.Flee) Utility.DelayAction.Add((MenuConfig.Qd * 10) + 1, Reset);
                    break;
                case "Spell1b":
                    Logic.LastQ = Utils.GameTimeTickCount;
                    if (MenuConfig.Qstrange && Orb.ActiveMode != Orbwalking.OrbwalkingMode.None) Game.Say("/d");
                    Qstack = 3;
                    if (Orb.ActiveMode != Orbwalking.OrbwalkingMode.None && Orb.ActiveMode != Orbwalking.OrbwalkingMode.LastHit && Orb.ActiveMode != Orbwalking.OrbwalkingMode.Flee) Utility.DelayAction.Add((MenuConfig.Qd * 10) + 1, Reset);
                    break;
                case "Spell1c":
                    Logic.LastQ = Utils.GameTimeTickCount;
                    if (MenuConfig.Qstrange && Orb.ActiveMode != Orbwalking.OrbwalkingMode.None) Game.Say("/d");
                    Qstack = 1;
                    if (Orb.ActiveMode != Orbwalking.OrbwalkingMode.None && Orb.ActiveMode != Orbwalking.OrbwalkingMode.LastHit && Orb.ActiveMode != Orbwalking.OrbwalkingMode.Flee) Utility.DelayAction.Add((MenuConfig.Qld * 10) + 3, Reset);
                    break;
                case "Spell3":
                    if ((Orb.ActiveMode == Orbwalking.OrbwalkingMode.Burst ||
                        Orb.ActiveMode == Orbwalking.OrbwalkingMode.Combo ||
                        Orb.ActiveMode == Orbwalking.OrbwalkingMode.FastHarass ||
                        Orb.ActiveMode == Orbwalking.OrbwalkingMode.Flee)) ITEMS.CastYoumoo();
                    break;
                case "Spell4a":
                    Logic.LastR = Utils.GameTimeTickCount;
                    break;
                case "Spell4b":
                    var target = TargetSelector.GetSelectedTarget();
                    if (Spells.Q.IsReady() && target.IsValidTarget()) Logic.ForceQ(target);
                    break;
            }
        }
        private static void Reset()
        {
            Orbwalking.LastAATick = 0;
            if (MenuConfig.QReset) Game.Say("/d");
            Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position.Extend(Game.CursorPos, Player.Distance(Game.CursorPos) + 10));
            Player.IssueOrder(GameObjectOrder.MoveTo,
                 Player.Position - 115);
        }
    }
}
