using LeagueSharp;
using LeagueSharp.Common;

namespace Nechrito_Rengar
{
    class Spells
    {
        private static Obj_AI_Hero Player = ObjectManager.Player;
        public static SpellSlot Ignite;
        public static Spell _q, _w, _e, _r;
        public static void Initialise()
        {
            _q = new Spell(SpellSlot.Q);
            _w = new Spell(SpellSlot.W, 300);
            _e = new Spell(SpellSlot.E, 900f);
            _e.SetSkillshot(0.125f, 70, 1500f, true, SkillshotType.SkillshotLine);
            _r = new Spell(SpellSlot.R);
            Ignite = Player.GetSpellSlot("SummonerDot");
        }


    }
}