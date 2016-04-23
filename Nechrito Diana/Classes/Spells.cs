using LeagueSharp;
using LeagueSharp.Common;

namespace Nechrito_Diana
{
    class Spells
    {
        public static SpellSlot Ignite, Flash;
        public static Spell _q, _w, _e, _r;
        private static Obj_AI_Hero Player = ObjectManager.Player;
        public static void Initialise()
        {
            _q = new Spell(SpellSlot.Q, 825f);
             _q.SetSkillshot(0.25f, 185, 1640, false, SkillshotType.SkillshotCone);
            _w = new Spell(SpellSlot.W);
            _e = new Spell(SpellSlot.E, 450);
            _r = new Spell(SpellSlot.R, 825);

            Ignite = Player.GetSpellSlot("SummonerDot");
            Flash = Player.GetSpellSlot("SummonerFlash");
        }
    }
}
