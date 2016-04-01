using LeagueSharp;
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
            if (Program.Player.GetSpell(Program.Smite).Name.ToLower() == "s5_summonersmiteplayerganker")
                {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._e.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    Program.Player.Spellbook.CastSpell(Program.Smite, Program.Player);
                }
            }
            foreach (var minion in MinionManager.GetMinions(900f, MinionTypes.All, MinionTeam.Neutral))
            {
                var damage = Program.Player.Spellbook.GetSpell(Program.Smite).State == SpellState.Ready
                     ? (float)Program.Player.GetSummonerSpellDamage(minion, Damage.SummonerSpell.Smite)
                : 0;
                if (minion.Distance(Program.Player.ServerPosition) <= 500)
                {
                    if ((minion.CharData.BaseSkinName.Contains("Dragon") || minion.CharData.BaseSkinName.Contains("Baron")) && (damage >= minion.Health))
                    {
                        Program.Player.Spellbook.CastSpell(Program.Smite, minion);
                    }

                }
            }
        }
    }
}
