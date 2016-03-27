
using LeagueSharp;
using LeagueSharp.Common;

namespace NechritoRiven
{
    class Burst
    {

        public static void DoBurst()
        {
            var target = TargetSelector.GetSelectedTarget();

            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {

                // Else R wont cast, Bug
                if (Spells._r.IsReady() && Spells._r.Instance.Name == Program.IsFirstR && MenuConfig.AlwaysR && Program.Player.Distance(target.Position) <= Spells._e.Range + (Program.Player.AttackRange) &&
               target != null)
                    Program.ForceR();

                // Flash
                if ((Program.Player.Distance(target.Position) <= 700) && (  Program.Player.Distance(target.Position) >= 600) && Program.Player.Spellbook.GetSpell(Spells.Flash).State == SpellState.Ready && Spells._r.IsReady() && Spells._e.IsReady() && Spells._w.IsReady() && target.Health < Program.Totaldame(target))
                {
                    Spells._e.Cast(target.Position);
                    Program.ForceR();
                    Program.CastYoumoo();
                    Utility.DelayAction.Add(65, () => Program.Player.Spellbook.CastSpell(Spells.Flash, target.Position));
                }
                // Burst
                if (Spells._e.IsReady() && Spells._w.IsReady() &&
                        (Program.Player.Distance(target.Position) <= Spells._e.Range + (Program.Player.AttackRange)))
                {
                    Spells._e.Cast(target.ServerPosition);
                    Program.ForceR();
                    Program.CastYoumoo();
                    Utility.DelayAction.Add(70, Program.ForceItem);
                    Utility.DelayAction.Add(120, Program.ForceW);
                    Spells._r.Cast(target.ServerPosition);
                    Utility.DelayAction.Add(30, () => Program.ForceCastQ(target));
                }

            }
        }
    }
}
