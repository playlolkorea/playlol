using LeagueSharp;
using LeagueSharp.Common;
using System.Linq;

namespace Nechrito_Rengar
{
    class Killsteal
    {
       public static void _Killsteal()
        {
            if (Spells._q.IsReady())
            {
                // R range because auto-gapclose! (Yes, i'm smart. Give Contrib pls)
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._q.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Spells._q.GetDamage(target))
                        Spells._q.Cast(target);
                }
            }
            if (Spells._w.IsReady())
            {
                // R range because auto-gapclose! (Yes, i'm smart. Give Contrib pls)
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._w.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Spells._w.GetDamage(target))
                        Spells._w.Cast(target);
                }
            }
            if (Spells._e.IsReady())
            {
                // R range because auto-gapclose! (Yes, i'm smart. Give Contrib pls)
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._e.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Spells._e.GetDamage(target))
                        Spells._e.Cast(target);
                }
            }
        }
    }
}
