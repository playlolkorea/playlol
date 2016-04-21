using LeagueSharp;
using LeagueSharp.Common;

namespace Nechrito_Diana
{
    class Dmg
    {
        
        public static float ComboDmg(Obj_AI_Base enemy)
        {
            if (enemy != null) 
                return 0;

            float damage = 0;
            if (Spells._w.IsReady()) damage += Spells._w.GetDamage(enemy);
                
            if(Program.Player.HasBuff("dianapassivebuff"))
            {
                if (Spells._r.IsReady() && Spells._q.IsReady()) 
                    damage += Spells._q.GetDamage(enemy) + Spells._r.GetDamage(enemy) + 
                        Spells._r.GetDamage(enemy) + (float)Program.Player.GetAutoAttackDamage(enemy);
            }
                
            if (Spells._q.IsReady()) damage += Spells._q.GetDamage(enemy);
            if(Spells._r.IsReady()) damage += Spells._r.GetDamage(enemy);
                
            return damage;
        }
        
        public static bool IsLethal(Obj_AI_Base unit)
        {
            // why 1.65 ? Why not Zero Kappa
            return ComboDmg(unit) / 1.65 >= unit.Health;
        }
        
    }
}
