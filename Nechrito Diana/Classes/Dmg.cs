using LeagueSharp;
using LeagueSharp.Common;

namespace Nechrito_Diana
{
    class Dmg
    {
        public static float ComboDmg(Obj_AI_Base enemy)
        {
            if (enemy != null)
            {
                float damage = 0;
                if (Spells._w.IsReady()) damage = damage + Spells._w.GetDamage(enemy);
                if(Program.Player.HasBuff("dianapassivebuff"))
                {
                    if (Spells._r.IsReady() && Spells._q.IsReady()) damage = damage + Spells._q.GetDamage(enemy) + Spells._r.GetDamage(enemy) + Spells._r.GetDamage(enemy) + (float)Program.Player.GetAutoAttackDamage(enemy);
                }
                if (Spells._q.IsReady()) damage = damage + Spells._q.GetDamage(enemy);
                if(Spells._r.IsReady()) damage = damage + Spells._r.GetDamage(enemy);
                return damage;
            }
            return 0;
        }
        public static bool IsLethal(Obj_AI_Base unit)
        {
            return ComboDmg(unit) / 1.65 >= unit.Health;
        }
    }
}
