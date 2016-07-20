using LeagueSharp;
using LeagueSharp.Common;

namespace ReformedAIO.Champions.Ashe.Logic
{
    class QLogic
    {
        public int QCount()
        {
            return Variable.Player.GetBuffCount("AsheQ");
        }

        public void Kite(Obj_AI_Base x)
        {
            if(x == null || x.Distance(Variable.Player) > Variable.Player.AttackRange || x.HasBuffOfType(BuffType.PhysicalImmunity)) return;

            Variable.Orbwalker.ForceTarget(x);
        }
    }
}
