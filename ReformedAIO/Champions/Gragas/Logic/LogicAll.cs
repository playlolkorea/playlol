using LeagueSharp;
using LeagueSharp.Common;

namespace ReformedAIO.Champions.Gragas.Logic
{
    internal class LogicAll
    {
        public float ComboDmg(Obj_AI_Base x)
        {
            var Dmg = 0f;

            if (Variable.Spells[SpellSlot.Q].IsReady()) Dmg = Dmg + Variable.Spells[SpellSlot.Q].GetDamage(x);

            if (Variable.Spells[SpellSlot.W].IsReady()) Dmg = Dmg + Variable.Spells[SpellSlot.W].GetDamage(x) + (float)Variable.Player.GetAutoAttackDamage(x);

            if (Variable.Spells[SpellSlot.E].IsReady()) Dmg = Dmg + Variable.Spells[SpellSlot.E].GetDamage(x);

            if (Variable.Spells[SpellSlot.R].IsReady()) Dmg = Dmg + Variable.Spells[SpellSlot.R].GetDamage(x);

            if (Variable.Player.HasBuff("SummonerExhaust")) Dmg = Dmg * 0.6f;

            return Dmg;
        }
    }
}
