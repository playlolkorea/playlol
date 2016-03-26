using LeagueSharp.Common;
using LeagueSharp;

namespace Nechrito_Twitch
{
    class Spells
    {
        private static Obj_AI_Hero Player = ObjectManager.Player;
        public static SpellSlot Ignite, Flash;
        public static Spell _q, _q2, _q3, _w, _e, _r, _recall;

        public static void Initialise()
        {
            _w = new Spell(SpellSlot.W, 950);
           _w.SetSkillshot(0.25f, 120f, 1400f, false, SkillshotType.SkillshotCircle);
            _e = new Spell(SpellSlot.E, 1200);
            _r = new Spell(SpellSlot.R, 900);
            _recall = new Spell(SpellSlot.Recall);
        }
    }
}
