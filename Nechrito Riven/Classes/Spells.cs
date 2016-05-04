using LeagueSharp;
using LeagueSharp.Common;

namespace NechritoRiven
{
    class Spells : Riven
    {
        public static SpellSlot Ignite, Flash;
        public static Spell _q { get; set; }
        public static Spell _w { get; set; }
        public static Spell _e { get; set; }
        public static Spell _r { get; set; }
        public static void Initialise()
        {
            _q = new Spell(SpellSlot.Q, 260f);
            _w = new Spell(SpellSlot.W, 250f);
            _e = new Spell(SpellSlot.E, 270);
            _r = new Spell(SpellSlot.R, 900);

            _q.SetSkillshot(0.25f, 100f, 2200f, false, SkillshotType.SkillshotCircle);
            _r.SetSkillshot(0.25f, (float)(45 * 0.5), 1600, false, SkillshotType.SkillshotCone);

            Ignite = Player.GetSpellSlot("SummonerDot");
            Flash = Player.GetSpellSlot("SummonerFlash");

        }


    }
}