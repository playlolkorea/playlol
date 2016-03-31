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
            var target = TargetSelector.GetSelectedTarget();
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                if (Spells._q.IsReady() && Spells._w.IsReady() && Spells._e.IsReady() && Program.Player.Mana == 5 && MenuConfig.OneShot)
                {
                    Logic.CastYoumoo();
                    Spells._q.Cast(target);
                    Spells._e.Cast(target);
                    Spells._w.Cast(target);


                }
                if (Spells._q.IsReady() && Spells._w.IsReady() && Spells._e.IsReady() && Program.Player.Mana <= 4 && MenuConfig.GankCombo)
                {
                    Logic.CastYoumoo();
                    Spells._w.Cast(target);
                    Spells._e.Cast(target);
                    Spells._q.Cast(target);


                }
                if (Spells._q.IsReady() && Spells._w.IsReady() && Spells._e.IsReady() && Program.Player.Mana == 5 && MenuConfig.GankCombo)
                {
                    Logic.CastYoumoo();
                    Spells._e.Cast(target);
                    Spells._q.Cast(target);
                    Spells._w.Cast(target);

                }
            }
        }
        
    }
}
