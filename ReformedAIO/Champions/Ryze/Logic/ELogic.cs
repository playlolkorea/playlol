using LeagueSharp;

namespace ReformedAIO.Champions.Ryze.Logic
{
    internal class ELogic
    {
        public bool RyzeE(Obj_AI_Base x)
        {
            return x.HasBuff("RyzeE");
        }

        public bool StageFirst()
        {
            return Variable.Player.HasBuff("ryzeqiconnocharge");
        }

        public bool StageHalf()
        {
            return Variable.Player.HasBuff("ryzeqiconhalfcharge");
        }

        public bool StageFull()
        {
            return Variable.Player.HasBuff("ryzeqiconfullcharge");
        }
    }
}
