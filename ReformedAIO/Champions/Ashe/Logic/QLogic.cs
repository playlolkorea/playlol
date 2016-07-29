using LeagueSharp;

namespace ReformedAIO.Champions.Ashe.Logic
{
    internal class QLogic
    {
        public int QCount()
        {
            return Variable.Player.GetBuffCount("AsheQ");
        }

        public void Kite(Obj_AI_Base x)
        {
            if(x == null || x.HasBuffOfType(BuffType.PhysicalImmunity)) return;

            Variable.Orbwalker.ForceTarget(x);
        }
    }
}
