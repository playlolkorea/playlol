using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Diana;

namespace ReformedAIO.Champions.Ryze.Logic
{
    internal class Damage
    {
        public float ComboDmg(Obj_AI_Base x)
        {
            var Dmg = 0f;

            if (Variable.Spells[SpellSlot.Q].IsReady()) Dmg = Dmg + Variables.Spells[SpellSlot.Q].GetDamage(x) * 2;

            if (Variable.Spells[SpellSlot.W].IsReady()) Dmg = Dmg + Variables.Spells[SpellSlot.W].GetDamage(x);

            if (Variable.Spells[SpellSlot.E].IsReady()) Dmg = Dmg + Variables.Spells[SpellSlot.E].GetDamage(x);

            if (Variable.Spells[SpellSlot.R].IsReady()) Dmg = Dmg + Variables.Spells[SpellSlot.R].GetDamage(x);

            if (Variable.Player.HasBuff("SummonerExhaust")) Dmg = Dmg * 0.6f;

            return Dmg;
        }
    }
}
