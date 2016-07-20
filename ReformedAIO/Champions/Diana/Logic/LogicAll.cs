using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.TargetValidator;
using RethoughtLib.TargetValidator.Interfaces;

namespace ReformedAIO.Champions.Diana.Logic
{
    class LogicAll
    {
        private TargetValidator targetValidator;

        private readonly List<ICheckable> check = new List<ICheckable>() { new OrbwalkingMode.Target.IsDead(), new OrbwalkingMode.Target.CheckTargetCheck(), new OrbwalkingMode.Target.IsNullCheck() };

        public bool LegitTarget(Obj_AI_Base x)
        {
            targetValidator = new TargetValidator();

            targetValidator.Reset();

            targetValidator.AddChecks(check);

            var c = targetValidator.Check(x);

            return c;
        }

        public float ComboDmg(Obj_AI_Base x)
        {
            var Dmg = 0f;

            if (Variables.Spells[SpellSlot.Q].IsReady()) Dmg = Dmg + Variables.Spells[SpellSlot.Q].GetDamage(x);

            if (Variables.Spells[SpellSlot.W].IsReady()) Dmg = Dmg + Variables.Spells[SpellSlot.W].GetDamage(x);

            if (Variables.Spells[SpellSlot.E].IsReady()) Dmg = Dmg + Variables.Spells[SpellSlot.E].GetDamage(x);

            if (Variables.Spells[SpellSlot.R].IsReady()) Dmg = Dmg + Variables.Spells[SpellSlot.R].GetDamage(x);

            if (Variables.Player.HasBuff("SummonerExhaust")) Dmg = Dmg * 0.6f;

            return Dmg;
        }
    }
}
