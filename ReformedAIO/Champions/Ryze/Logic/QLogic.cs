using LeagueSharp;
using SharpDX;

namespace ReformedAIO.Champions.Ryze.Logic
{
    internal class QLogic
    {
        public Vector3 QPred(Obj_AI_Base x)
        {
            var pos = Variable.Spells[SpellSlot.Q].GetPrediction(x);


            return pos.CastPosition;
        }
    }
}
