using LeagueSharp.Common;
using System.Linq;

namespace Nechrito_Rengar
{
    class Killsteal
    {
       public static void _Killsteal()
        {
            if (Spells._w.IsReady())
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._w.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Spells._w.GetDamage(target))
                        Spells._w.Cast(target);
                }
            }
            if (Spells._e.IsReady())
            {
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
