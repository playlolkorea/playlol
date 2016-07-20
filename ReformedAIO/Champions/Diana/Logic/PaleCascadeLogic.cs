using LeagueSharp;
using LeagueSharp.Common;

namespace ReformedAIO.Champions.Diana.Logic
{
    class PaleCascadeLogic
    {
        protected Obj_AI_Hero Target;

        public bool tBuff(Obj_AI_Base x)
        {
            return x.HasBuff("dianamoonlight");
        }

        public float GetDmg(Obj_AI_Base x)
        {
            if (x == null) { return 0; }

            var Dmg = 0f;

            if (Variables.Player.HasBuff("SummonerExhaust")) Dmg = Dmg * 0.6f;

            if (Variables.Spells[SpellSlot.Q].IsReady()) Dmg = Dmg + Variables.Spells[SpellSlot.R].GetDamage(x);

            return Dmg;
        }
    }
}
