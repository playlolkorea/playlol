using LeagueSharp.Common;
using LeagueSharp;
using SharpDX;

namespace Nechrito_Gragas
{
    class Mode
    {
        public static float GetKnockBackRange(Vector3 to, Vector3 from)
        {
            return Spells._r.Range - from.Distance(to);
        }
        public static Vector3 GetPredictedBarellPosition(Obj_AI_Hero target)
        {
            var result = new Vector3();

            if (target.IsValid)
            {
                var etaR = Program.Player.Distance(target) / Spells._r.Speed;
                var pred = Prediction.GetPrediction(target, etaR);

                result = Geometry.Extend(pred.UnitPosition, target.ServerPosition, GetKnockBackRange(target.ServerPosition, pred.UnitPosition));
            }
            return result;
        }
        private static Obj_AI_Hero Player => ObjectManager.Player;
        public static void ComboLogic()
        {
            var Target = TargetSelector.GetSelectedTarget();
            var predQ = GetPredictedBarellPosition(Target);
            
            if (Target != null && Target.IsValidTarget() && !Target.IsZombie && (Program.Player.Distance(Target.Position) <= 950))
                {
                if (Spells._q.IsReady() && Spells._r.IsReady() && !Target.IsDashing())
                {
                    Spells._q.Cast(predQ);
                    Spells._r.Cast(Target.ServerPosition + 100);
                }
                else if (Spells._q.IsReady() && !Spells._r.IsReady())
                    Spells._q.Cast(Target.ServerPosition);
                // E
                else if (Spells._e.IsReady() && (Program.Player.Distance(Target.Position) <= 970))
                    Spells._e.Cast(Target.ServerPosition);       
                
                // Smite
              else  if (Spells.Smite != SpellSlot.Unknown && Spells._r.IsReady()
                       && Player.Spellbook.CanUseSpell(Spells.Smite) == SpellState.Ready && !Target.IsZombie)
                    Player.Spellbook.CastSpell(Spells.Smite, Target);
               // W
             else  if (Spells._w.IsReady() && !Spells._e.IsReady())
                       Spells._w.Cast();

                {
                    var target = TargetSelector.GetTarget(700f, TargetSelector.DamageType.Magical);
                    if (target != null && target.IsValidTarget() && !target.IsZombie)
                    {
                        if (Spells.Smite != SpellSlot.Unknown
                           && Player.Spellbook.CanUseSpell(Spells.Smite) == SpellState.Ready && !target.IsZombie)
                        {
                            Player.Spellbook.CastSpell(Spells.Smite, target);
                        }
                        if (Spells._e.IsReady() && !target.IsDashing())
                            Spells._e.Cast(target);
                        if (Spells._q.IsReady() && !target.IsDashing())
                            Spells._q.Cast(Player.Position.Extend(target.Position, Player.Distance(target)));
                        else if (Spells._q.IsReady() && !target.IsDashing())
                            Spells._q.Cast(target.Position);
                        else if (Spells._w.IsReady())
                            Spells._w.Cast();

                         // FLASH
                        //   if (Spells.Flash != SpellSlot.Unknown
                       //   && Player.Spellbook.CanUseSpell(Spells.Flash) == SpellState.Ready && !Target.IsZombie && (Program.Player.Distance(Target.Position) >= 600) && (Program.Player.Distance(Target.Position) <= 800))
                      //    Player.Spellbook.CastSpell(Spells.Flash, Target);

                    }

                }
            }
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
