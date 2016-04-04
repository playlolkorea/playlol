using LeagueSharp.Common;
using LeagueSharp;

namespace Nechrito_Gragas
{
    class Mode
    {
        private static Obj_AI_Hero Player => ObjectManager.Player;
        public static void ComboLogic()
        {
            var target = TargetSelector.GetTarget(400f, TargetSelector.DamageType.Magical);
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                if (Spells.Smite != SpellSlot.Unknown
                   && Player.Spellbook.CanUseSpell(Spells.Smite) == SpellState.Ready && !target.IsZombie)
                {
                    Player.Spellbook.CastSpell(Spells.Smite, target);
                }
                 if (Spells._e.IsReady() && !target.IsDashing())
                     Spells._e.Cast(target);
                 if (Spells._r.IsReady())
                     Spells._r.Cast(Player.Position.Extend(target.Position, Player.Distance(target) + 100f));
                 if (Spells._q.IsReady() && !target.IsDashing())
                     Spells._q.Cast(Player.Position.Extend(target.Position, Player.Distance(target)));
           else  if (Spells._w.IsReady())
                     Spells._w.Cast();
            }
        }
        public static void InsecLogic()
        {
            var target = TargetSelector.GetTarget(Spells._e.Range - 80, TargetSelector.DamageType.Magical);
            // BACKUP COMBO!!
            if (Spells._e.IsReady())
                Spells._e.Cast(target);
            if (Spells._r.IsReady())
                Spells._r.Cast(target.ServerPosition + 100);
            if (Spells._q.IsReady())
                Spells._q.Cast(target);
            if (Spells._w.IsReady() && !Spells._r.IsReady())
                Spells._w.Cast();
        }
        public static void JungleLogic()
        {
            var mobs = MinionManager.GetMinions(190 + Player.AttackRange, MinionTypes.All, MinionTeam.Neutral,
          MinionOrderTypes.MaxHealth);

            if (mobs.Count <= 0)
                return;
            if (Spells._w.IsReady())
                Spells._w.Cast();
            if (Spells._e.IsReady())
                Spells._e.Cast(mobs[0]);
            if (Spells._q.IsReady())
               Spells._q.Cast(mobs[0]);             
        }
        public static void HarassLogic()
        {
            var target = TargetSelector.GetTarget(Spells._r.Range - 50, TargetSelector.DamageType.Magical);
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                if (Spells._e.IsReady())
                    Spells._e.Cast(target);
                if (Spells._q.IsReady())
                    Spells._q.Cast(target);
            }
        }
    }
}
