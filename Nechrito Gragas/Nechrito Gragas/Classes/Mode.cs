using LeagueSharp.Common;
using LeagueSharp;
using SharpDX;

namespace Nechrito_Gragas
{
    class Mode
    {
        private float GetKnockBackRange(Vector3 from, Vector3 to)
        {
            return Spells._r.Range - from.Distance(to);
        }
        private Vector3 GetPredictedBarellPosition(Obj_AI_Hero target)
        {
            var result = new Vector3();

            if (target.IsValid)
            {
                var etaR = Program.Player.Distance(target) / Spells._r.Speed;
                var pred = Prediction.GetPrediction(target, etaR);

                result = Geometry.Extend(pred.UnitPosition, target.ServerPosition, GetKnockBackRange(pred.UnitPosition, target.ServerPosition));
            }
            return result;
        }
        private static Obj_AI_Hero Player => ObjectManager.Player;
        public static void ComboLogic()
        {
            
                var Target = TargetSelector.GetSelectedTarget();
                // Make this selected Target. (Change key?)
                // SELECTED TARGET == FLASH COMBO (DELETE FROM MENUCONFIG)!!!!
                
                if (Target != null && Target.IsValidTarget() && !Target.IsZombie && (Program.Player.Distance(Target.Position) <= 950))
                {
                // E
                if (Spells._e.IsReady() && (Program.Player.Distance(Target.Position) <= 970))
                    Spells._e.Cast(Target);
                // RQ
                if (Spells._r.IsReady() && Target.IsFacing(Player))
                    Spells._r.Cast(Player.Position.Extend(Target.Position, Player.Distance(Target) + 90f));                   
                
                if (Spells._r.IsReady() && !Target.IsFacing(Player))
                {
                    Spells._e.Cast(Target);
                    Spells._r.Cast(Player.Position.Extend(Target.Position, Player.Distance(Target) + 120f));
                }
                // Smite
                if (Spells.Smite != SpellSlot.Unknown && Spells._r.IsReady()
                       && Player.Spellbook.CanUseSpell(Spells.Smite) == SpellState.Ready && !Target.IsZombie)
                    Player.Spellbook.CastSpell(Spells.Smite, Target);
                // Q If no r
                      
                    if (Spells._q.IsReady())
                        Spells._q.Cast(Target.Position);
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
                        if (Spells._r.IsReady())
                            Spells._r.Cast(Player.Position.Extend(target.Position, Player.Distance(target) + 100f));
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
