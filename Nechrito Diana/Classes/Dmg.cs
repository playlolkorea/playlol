using LeagueSharp;
using LeagueSharp.Common;

namespace Nechrito_Diana
{
    class Dmg
    {
<<<<<<< HEAD
        public static float SmiteDamage(Obj_AI_Hero target)
        {
            if (Logic.Smite == SpellSlot.Unknown || Program.Player.Spellbook.CanUseSpell(Logic.Smite) != SpellState.Ready)
            {
                return 0f;
            }
            return (float)Program.Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Smite);
        }
        public static float IgniteDamage(Obj_AI_Hero target)
        {
            if (Spells.Ignite == SpellSlot.Unknown || Program.Player.Spellbook.CanUseSpell(Spells.Ignite) != SpellState.Ready)
            {
                return 0f;
            }
            return (float)Program.Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite);
        }
=======
        
>>>>>>> 711b82681b9eac350bab4dcd046c843e6bd6b5c9
        public static float ComboDmg(Obj_AI_Base enemy)
        {
            if (enemy != null) 
                return 0;

            float damage = 0;
            if (Spells._w.IsReady()) damage += Spells._w.GetDamage(enemy);
                
            if(Program.Player.HasBuff("dianapassivebuff"))
            {
<<<<<<< HEAD
                float damage = 0;
                if (Program.Player.Masteries.Equals("thunderlordsdecree")) damage += (float)Program.Player.GetAutoAttackDamage(enemy) * (1.15f);
                // dianapassivebuff or dianamoonlight, cba to actually check yet
                if (Program.Player.HasBuff("dianamoonlight"))
                {
                    if (Spells._r.IsReady() && Spells._q.IsReady())
                        damage += Spells._q.GetDamage(enemy) + Spells._r.GetDamage(enemy) +
                            Spells._r.GetDamage(enemy) + (float)Program.Player.GetAutoAttackDamage(enemy);
                }
                damage = damage + (float)Program.Player.GetAutoAttackDamage(enemy);
                if (Spells._q.IsReady()) damage += Spells._q.GetDamage(enemy);
                if (Spells._w.IsReady()) damage += Spells._w.GetDamage(enemy);
                if (Spells._r.IsReady()) damage += Spells._r.GetDamage(enemy);
                return damage;
=======
                if (Spells._r.IsReady() && Spells._q.IsReady()) 
                    damage += Spells._q.GetDamage(enemy) + Spells._r.GetDamage(enemy) + 
                        Spells._r.GetDamage(enemy) + (float)Program.Player.GetAutoAttackDamage(enemy);
>>>>>>> 711b82681b9eac350bab4dcd046c843e6bd6b5c9
            }
                
            if (Spells._q.IsReady()) damage += Spells._q.GetDamage(enemy);
            if(Spells._r.IsReady()) damage += Spells._r.GetDamage(enemy);
                
            return damage;
        }
        
        public static bool IsLethal(Obj_AI_Base unit)
        {
<<<<<<< HEAD
            return ComboDmg(unit) / 1.75 >= unit.Health;
=======
            // why 1.65 ? Why not Zero Kappa
            return ComboDmg(unit) / 1.65 >= unit.Health;
>>>>>>> 711b82681b9eac350bab4dcd046c843e6bd6b5c9
        }
        
    }
}
