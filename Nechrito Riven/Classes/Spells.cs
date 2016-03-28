using LeagueSharp;
using LeagueSharp.Common;

namespace NechritoRiven
{
    class Spells
    {
        public const string IsFirstR = "RivenFengShuiEngine";
        public const string IsSecondR = "RivenIzunaBlade";
        private static Obj_AI_Hero Player = ObjectManager.Player;
        public static SpellSlot Ignite, Flash;
        public static Spell _q, _q2, _q3, _w, _e, _r;
        public static bool _forceR;
        public static bool _forceR2;
        public static void Initialise()
        {
            _q = new Spell(SpellSlot.Q, 260f);
            _q.SetSkillshot(0.25f, 100f, 2200f, false, SkillshotType.SkillshotCircle);
            _w = new Spell(SpellSlot.W, 250f);
            _e = new Spell(SpellSlot.E, 270);
            _r = new Spell(SpellSlot.R, 900);
            _r.SetSkillshot(0.25f, (float)(45 * 0.5), 1600, false, SkillshotType.SkillshotCircle);
            Ignite = Player.GetSpellSlot("SummonerDot");
            Flash = Player.GetSpellSlot("SummonerFlash");

        }


    }
}