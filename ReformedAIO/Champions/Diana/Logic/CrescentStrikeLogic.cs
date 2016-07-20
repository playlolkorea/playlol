using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SPrediction;

namespace ReformedAIO.Champions.Diana.Logic
{
    class CrescentStrikeLogic
    {
        protected Obj_AI_Hero Target;

        public Vector3 QPred(Obj_AI_Hero target)
        {
            var pos = Variables.Spells[SpellSlot.Q].GetArcSPrediction(target);

            // TODO Check how this can be improved.
            
           
            return pos.CastPosition.To3D() + QDelay(target);
        }

        public float QDelay(Obj_AI_Hero target)
        {
            var time = target.Distance(Variables.Player)/Variables.Spells[SpellSlot.Q].Speed;

            return time + Variables.Spells[SpellSlot.Q].Delay;
        }

        //public float misayaDelay(Obj_AI_Hero target)
        //{
        //    var time = target.Distance(Variables.Player) / (Variables.Player.MoveSpeed + Variables.Spells[SpellSlot.Q].Speed);

        //    return time + Variables.Spells[SpellSlot.Q].Delay;
        //}

        public float misayaDelay(Obj_AI_Hero target)
        {
            var dist = Variables.Player.ServerPosition.Distance(target.ServerPosition);
            var delay = Variables.Spells[SpellSlot.Q].Delay;
            var speed = Variables.Spells[SpellSlot.Q].Speed;
            var movespeed = Variables.Player.MoveSpeed;

            var time = 0f;

            if (dist > delay) // Impossible to do it otherwise
            {

                time = (dist / (movespeed + speed));
            }

            return (time + delay);
        }

        public float GetDmg(Obj_AI_Base x)
        {
            if(x == null) { return 0; }

            var Dmg = 0f;

            if (Variables.Player.HasBuff("SummonerExhaust")) Dmg = Dmg*0.6f;

            if (Variables.Spells[SpellSlot.Q].IsReady()) Dmg = Dmg + Variables.Spells[SpellSlot.Q].GetDamage(x);

            return Dmg;
        }
    }
}
