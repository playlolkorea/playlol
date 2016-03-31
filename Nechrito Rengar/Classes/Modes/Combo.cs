using System;
using LeagueSharp.Common;


namespace Nechrito_Rengar
{
    class Combo
    {
        private static void Game_OnUpdate(EventArgs args)
        {
            ComboLogic();
        }
        public static void ComboLogic()
        {
            var target = TargetSelector.GetTarget(Program.Player.AttackRange + 240f, TargetSelector.DamageType.Physical);
            if (Spells._q.IsReady() && Spells._w.IsReady() && Spells._e.IsReady() && Program.Player.Mana == 5)
            {
                Spells._q.Cast(target);
                Spells._e.Cast(target);
                Logic.CastTitan();
                Logic.ForceItem();
                Spells._w.Cast(target);
                Spells._q.Cast(target);

            }
           else if(Spells._e.IsReady() && (Program.Player.Distance(target.Position) <= Spells._e.Range) && (Program.Player.Distance(target.Position) >= Program.Player.AttackRange))
            {
                Spells._e.Cast(target.ServerPosition);
            }


        }
        
    }
}
