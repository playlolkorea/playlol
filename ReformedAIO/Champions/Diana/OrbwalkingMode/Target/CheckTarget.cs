using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.TargetValidator.Interfaces;

namespace ReformedAIO.Champions.Diana.OrbwalkingMode.Target
{
    class CheckTargetCheck : ICheckable
    {
        public bool Check(Obj_AI_Base target)
        {
           return target.IsValid;
        }
    }

    class IsDead : ICheckable
    {
        public bool Check(Obj_AI_Base target)
        {
            return !target.IsDead;
        }
    }

    class IsNullCheck : ICheckable
    {
        public bool Check(Obj_AI_Base target)
        {
            return target != null;
        }
    }

    class underTurret : ICheckable
    {
        public bool Check(Obj_AI_Base target)
        {
            return target.UnderTurret();
        }
    }
}
