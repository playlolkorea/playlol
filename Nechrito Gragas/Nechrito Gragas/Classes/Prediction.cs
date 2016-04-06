using System;

namespace Nechrito_Gragas
{
    using LeagueSharp;
    using LeagueSharp.Common;
    using SharpDX;

    class Predictio
    {
        public void OnUpdate(EventArgs args)
        {
            Obj_AI_Hero target = null;

            // Some target dunno how you did that before
            target = TargetSelector.GetSelectedTarget();

            if (target != null)
            {
                var predQ = GetPredictedBarellPosition(target);

                // ... cast Q on predQ
            }
        }

        // Method 1
        public Vector3 GetPredictedBarellPosition(Obj_AI_Hero target)
        {
            var result = new Vector3();

            if (target.IsValid)
            {
                var etaR = Program.Player.Distance(target) / Spells._r.Speed;
                var pred = LeagueSharp.Common.Prediction.GetPrediction(target, etaR);

                result = Geometry.Extend(pred.UnitPosition, target.ServerPosition, GetKnockBackRange(pred.UnitPosition, target.ServerPosition));
            }

            return result;
        }

        // Method 2
        public Vector3 GetPredictedBarrellPosition2(Obj_AI_Hero target)
        {
            var result = new Vector3();

            if (target.IsValid)
            {
                var pred = Prediction.GetPrediction(target, Spells._r.Delay, Spells._r.Speed);

                result = Geometry.Extend(pred.CastPosition, target.ServerPosition, GetKnockBackRange(pred.CastPosition, target.ServerPosition));
            }

            return result;
        }

        public float GetKnockBackRange(Vector3 from, Vector3 to)
        {
            return Spells._r.Range - from.Distance(to);
        }
    }
}