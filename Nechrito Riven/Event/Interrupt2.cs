using LeagueSharp;
using LeagueSharp.Common;
using NechritoRiven.Core;
using NechritoRiven.Menus;

namespace NechritoRiven.Event
{
    class Interrupt2 : Core.Core
    {
        public static void OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (!MenuConfig.InterruptMenu ||  sender.Distance(Player) > Spells.W.Range || sender.IsInvulnerable) return;

            if (Spells.W.IsReady())
            {
                Spells.W.Cast(sender.ServerPosition);
            }
        }
    }
}
