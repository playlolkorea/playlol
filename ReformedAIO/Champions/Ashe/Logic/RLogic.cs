using LeagueSharp;
using LeagueSharp.Common;

namespace ReformedAIO.Champions.Ashe.Logic
{
    class RLogic
    {
        public bool SafeR(Obj_AI_Hero target)
        {
            var safe = target.Distance(Variable.Player) < 1500 && (target.CountAlliesInRange(1500) > target.CountEnemiesInRange(1500) ||
                                                                   Variable.Player.Health > target.Health || ComboDamage(target) > target.Health); 
            // This will count for more allies than enemies in 1500 units or if player health is more than targets health, can be improved.

            if (target.UnderTurret()) safe = false;

            if (target.HasBuff("kindredrnodeathbuff") || target.HasBuff("Chrono Shift") ||
                target.HasBuffOfType(BuffType.PhysicalImmunity)) safe = false;

            return safe;
        }

        public bool Killable(Obj_AI_Hero target)
        {
            return RDmg(target) > target.Health && target.Distance(Variable.Player) < 1500;
        }

        private int QCount()
        {
            return Variable.Player.GetBuffCount("AsheQ");
        }

        public float RDmg(Obj_AI_Hero target)
        {
            var dmg = 0f;

            if (!Variable.Spells[SpellSlot.R].IsReady()) return 0f;

            if (Variable.Spells[SpellSlot.Q].IsReady() || QCount() >= 3) dmg = dmg +
                    (float)Variable.Player.GetAutoAttackDamage(target) * 4 + Variable.Spells[SpellSlot.Q].GetDamage(target);

            else
            {
                dmg = dmg + (float) Variable.Player.GetAutoAttackDamage(target);
            } 

            dmg = dmg + Variable.Spells[SpellSlot.R].GetDamage(target);

            return dmg;
        }

        public float ComboDamage(Obj_AI_Hero target)
        {
            var dmg = 0f;

            if (Variable.Spells[SpellSlot.Q].IsReady()) dmg = dmg + Variable.Spells[SpellSlot.Q].GetDamage(target);

            if (Variable.Spells[SpellSlot.W].IsReady()) dmg = dmg + Variable.Spells[SpellSlot.W].GetDamage(target);

            if (Variable.Spells[SpellSlot.R].IsReady()) dmg = dmg + Variable.Spells[SpellSlot.R].GetDamage(target);

            dmg = dmg * 1.17f; // Calcs are always a bit wrong, 17% could secure a kill.

            if (Variable.Player.HasBuff("SummonerExhaust")) dmg = dmg * 0.6f;

            return dmg;
        }

    }
}
