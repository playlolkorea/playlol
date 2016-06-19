using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace Nechrito_Twitch
{
    class Dmg
    {
        private static readonly Obj_AI_Hero Player = ObjectManager.Player;
        public static float GetDamage(Obj_AI_Base target)
        {
            if (!Spells._e.IsReady()) return 0;
            if (target == null || !target.IsValidTarget()) return 0;
            if (target.IsInvulnerable || target.HasBuff("KindredRNoDeathBuff") || target.HasBuffOfType(BuffType.SpellShield)) return 0;

            float eDmg = 0;

            eDmg = eDmg + ERaw(target) + Passive(target);
            
            if (Player.HasBuff("SummonerExhaust")) eDmg = eDmg * 0.6f;

            return eDmg;
        }

        public static float ExploitDamage(Obj_AI_Base target)
        {
            if (!Spells._e.IsReady()) return 0;
            if (target == null || !target.IsValidTarget()) return 0;
            if (target.IsInvulnerable || target.HasBuff("KindredRNoDeathBuff") || target.HasBuffOfType(BuffType.SpellShield)) return 0;

            float eDmg = 0;
 
            eDmg = eDmg + ERaw(target);           

            if (Player.HasBuff("SummonerExhaust")) eDmg = eDmg * 0.6f;

            return eDmg;
        }

        public static float ERaw(Obj_AI_Base target)
        {
            return (float)Player.CalcDamage(target, Damage.DamageType.True,
                EStackDamage[Spells._e.Level - 1] * Stacks(target) +
                0.2 * Player.FlatMagicDamageMod +
                0.25 * Player.FlatPhysicalDamageMod +
                EBaseDamage[Spells._e.Level - 1]);
        }

        public static float Passive(Obj_AI_Base target)
        {
            float dmg = 6;

            if (Player.Level > 16) dmg = 6;
            if (Player.Level > 12) dmg = 5;
            if (Player.Level > 8) dmg = 4;
            if (Player.Level > 4) dmg = 3;
            if (Player.Level > 0) dmg = 2;

            return dmg * Stacks(target) * PassiveTime(target) - target.HPRegenRate * PassiveTime(target);
        }

        public static float PassiveTime(Obj_AI_Base target)
        {
            if (!target.HasBuff("twitchdeadlyvenom")) return 0;

            return Math.Max(0, target.GetBuff("twitchdeadlyvenom").EndTime) - Game.Time;
        }

        public static float Stacks(Obj_AI_Base target)
        {
            return target.GetBuffCount("TwitchDeadlyVenom");
        }

        public static float[] EStackDamage = { 15, 20, 25, 30, 35 };

        public static float[] EBaseDamage = { 20, 35, 50, 65, 80 };
    }
}
