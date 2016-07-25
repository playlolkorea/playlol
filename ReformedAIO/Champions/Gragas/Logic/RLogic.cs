using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SPrediction;

namespace ReformedAIO.Champions.Gragas.Logic
{
    internal class RLogic
    {
        public Vector3 RPred(Obj_AI_Hero target)
        {
            var pos = Variable.Spells[SpellSlot.R].GetVectorSPrediction(target, 1150).CastTargetPosition.Extend(Variable.Player.Position.To2D(), 65);
            
            return pos.To3D() + RDelay(target);
        }

        private float RDelay(Obj_AI_Base target)
        {
            var time = target.Distance(Variable.Player) / Variable.Spells[SpellSlot.R].Speed;

            return time + Variable.Spells[SpellSlot.R].Delay;
        }
    }
}
