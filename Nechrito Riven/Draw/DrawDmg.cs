using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using NechritoRiven.Core;
using NechritoRiven.Menus;
using SharpDX;

namespace NechritoRiven.Draw
{
    class DrawDmg
    {
        private static readonly HpBarIndicator Indicator = new HpBarIndicator();
        public static void DmgDraw(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget() && !ene.IsZombie))
            {
                if (MenuConfig.Dind)
                {
                    var EasyKill = Spells.Q.IsReady() && Dmg.IsLethal(enemy)
                       ? new ColorBGRA(0, 255, 0, 120)
                       : new ColorBGRA(255, 255, 0, 120);
                    Indicator.unit = enemy;
                    Indicator.drawDmg(Dmg.GetComboDamage(enemy), EasyKill);
                }
            }
        }
    }
}
