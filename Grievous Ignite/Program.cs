using LeagueSharp.Common;
using System;
using LeagueSharp;
using System.Linq;
using SharpDX;

namespace Grievous_Ignite
{
    class Program
    {
        private static Obj_AI_Hero Player => ObjectManager.Player;
        private static int IgniteDmg = 50 + 20 * Player.Level;
        private static readonly HpBarIndicator Indicator = new HpBarIndicator();
        private static void Main() => CustomEvents.Game.OnGameLoad += OnGameLoad;
        private static SpellSlot Ignite;

        private static void OnGameLoad(EventArgs args)
        {
            Drawing.OnEndScene += Drawing_OnEndScene;

            Ignite = Player.GetSpellSlot("SummonerDot");

           if(Player.Spellbook.CanUseSpell(Ignite) != SpellState.Ready)
            {
                Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Grievous Ignite</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Unsuccessful</font></b>");
                return;
            }

            Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Grievous Ignite</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Loaded</font></b>");
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            // if there are valid targts & target isn't dead execute the following code within the brackets
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget() && !ene.IsZombie))
            {
                if (!(IgniteDmg >= enemy.Health)) return;

                Indicator.unit = enemy;
                Indicator.drawDmg(IgniteDmg, new ColorBGRA(255, 204, 0, 170));
            }
        }
    }
}
