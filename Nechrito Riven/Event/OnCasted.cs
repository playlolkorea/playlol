#region

using LeagueSharp;
using LeagueSharp.Common;
using NechritoRiven.Core;

#endregion

namespace NechritoRiven.Event
{
    internal class OnCasted : Core.Core
    {
        public static void OnCasting(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsEnemy || sender.Type != Player.Type) return;
            var epos = Player.ServerPosition + (Player.ServerPosition - sender.ServerPosition).Normalized() * 300;

            if (!(Player.Distance(sender.ServerPosition) <= args.SData.CastRange)) return;
            switch (args.SData.TargettingType)
            {
                case SpellDataTargetType.Unit:

                    if (args.Target.NetworkId == Player.NetworkId)
                    {
                        if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit || Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear &&
                            !args.SData.Name.Contains("NasusW"))
                        {
                            if (Spells.E.IsReady()) Spells.E.Cast(epos);
                        }
                    }
                    break;
                case SpellDataTargetType.SelfAoe:

                    if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit || Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
                    {
                        if (Spells.E.IsReady()) Spells.E.Cast(epos);
                    }
                    break;
            }
            if (args.SData.Name.Contains("IreliaEquilibriumStrike"))
            {
                if (args.Target.NetworkId == Player.NetworkId)
                {
                    if (Spells.W.IsReady() && InWRange(sender)) Spells.W.Cast();
                    else if (Spells.E.IsReady()) Spells.E.Cast(epos);
                }
            }
            if (args.SData.Name.Contains("TalonCutthroat"))
            {
                if (args.Target.NetworkId == Player.NetworkId)
                {
                    if (Spells.W.IsReady()) Spells.W.Cast();
                }
            }
            if (args.SData.Name.Contains("RenektonPreExecute"))
            {
                if (args.Target.NetworkId == Player.NetworkId)
                {
                    if (Spells.W.IsReady()) Spells.W.Cast();
                }
            }
            if (args.SData.Name.Contains("GarenRPreCast"))
            {
                if (args.Target.NetworkId == Player.NetworkId)
                {
                    if (Spells.E.IsReady()) Spells.E.Cast(epos);
                }
            }

            if (args.SData.Name.Contains("GarenQAttack"))
            {
                if (args.Target.NetworkId == Player.NetworkId)
                {
                    if (Spells.E.IsReady()) Spells.E.Cast(Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position.Extend(Game.CursorPos, Player.Distance(Game.CursorPos) + 10)));
                }
            }

            if (args.SData.Name.Contains("XenZhaoThrust3"))
            {
                if (args.Target.NetworkId == Player.NetworkId)
                {
                    if (Spells.W.IsReady()) Spells.W.Cast();
                }
            }
            if (args.SData.Name.Contains("RengarQ"))
            {
                if (args.Target.NetworkId == Player.NetworkId)
                {
                    if (Spells.E.IsReady()) Spells.E.Cast();
                }
            }
            if (args.SData.Name.Contains("RengarPassiveBuffDash"))
            {
                if (args.Target.NetworkId == Player.NetworkId)
                {
                    if (Spells.E.IsReady()) Spells.E.Cast();
                }
            }
            if (args.SData.Name.Contains("RengarPassiveBuffDashAADummy"))
            {
                if (args.Target.NetworkId == Player.NetworkId)
                {
                    if (Spells.E.IsReady()) Spells.E.Cast();
                }
            }
            if (args.SData.Name.Contains("TwitchEParticle"))
            {
                if (args.Target.NetworkId == Player.NetworkId)
                {
                    if (Spells.E.IsReady()) Spells.E.Cast();
                }
            }
            if (args.SData.Name.Contains("FizzPiercingStrike"))
            {
                if (args.Target.NetworkId == Player.NetworkId)
                {
                    if (Spells.E.IsReady()) Spells.E.Cast(Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position.Extend(Game.CursorPos, Player.Distance(Game.CursorPos) + 10)));
                }
            }
            if (args.SData.Name.Contains("HungeringStrike"))
            {
                if (args.Target.NetworkId == Player.NetworkId)
                {
                    if (Spells.E.IsReady()) Spells.E.Cast();
                }
            }
            if (args.SData.Name.Contains("YasuoDash"))
            {
                if (args.Target.NetworkId == Player.NetworkId)
                {
                    if (Spells.E.IsReady()) Spells.E.Cast();
                }
            }
            if (args.SData.Name.Contains("KatarinaRTrigger"))
            {
                if (args.Target.NetworkId == Player.NetworkId)
                {
                    if (Spells.W.IsReady() && InWRange(sender)) Spells.W.Cast();
                    else if (Spells.E.IsReady()) Spells.E.Cast(Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position.Extend(Game.CursorPos, Player.Distance(Game.CursorPos) + 10)));
                }
            }
            if (args.SData.Name.Contains("KatarinaE"))
            {
                if (args.Target.NetworkId == Player.NetworkId)
                {
                    if (Spells.W.IsReady()) Spells.W.Cast();
                }
            }
            if (args.SData.Name.Contains("MonkeyKingQAttack"))
            {
                if (args.Target.NetworkId == Player.NetworkId)
                {
                    if (Spells.E.IsReady()) Spells.E.Cast();
                }
            }
            if (args.SData.Name.Contains("MonkeyKingSpinToWin"))
            {
                if (args.Target.NetworkId == Player.NetworkId)
                {
                    if (Spells.E.IsReady()) Spells.E.Cast(Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position.Extend(Game.CursorPos, Player.Distance(Game.CursorPos) + 10)));
                    else if (Spells.W.IsReady()) Spells.W.Cast();
                }
            }
        }
    }
}
