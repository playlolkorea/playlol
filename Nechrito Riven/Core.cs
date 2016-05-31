using System;
using LeagueSharp;
using LeagueSharp.Common;
using System.Collections.Generic;
using SharpDX;
using NechritoRiven.Classes.Modes;

namespace NechritoRiven
{
    class Core
    {
        public static Render.Text Timer, Timer2;

        public static int Qstack = 1;
        public static readonly HpBarIndicator Indicator = new HpBarIndicator();
        public static Obj_AI_Hero Player => ObjectManager.Player;
        public static Orbwalking.Orbwalker Orb { get; set; }
        public class Spells
        {
            public const string IsFirstR = "RivenFengShuiEngine";
            public const string IsSecondR = "RivenIzunaBlade";
            public static SpellSlot Ignite, Smite, Flash;
            public static Spell Q { get; set; }
            public static Spell W { get; set; }
            public static Spell E { get; set; }
            public static Spell R { get; set; }
            public static void Load()
            {
                Q = new Spell(SpellSlot.Q, 260f);
                W = new Spell(SpellSlot.W, 250f);
                E = new Spell(SpellSlot.E, 270);
                R = new Spell(SpellSlot.R, 900);

                Q.SetSkillshot(0.25f, 100f, 2200f, false, SkillshotType.SkillshotCircle);
                R.SetSkillshot(0.25f, (float)(45 * 0.5), 1600, false, SkillshotType.SkillshotCone);

                Ignite = Player.GetSpellSlot("SummonerDot");
                Flash = Player.GetSpellSlot("SummonerFlash");
            }
        }
    }
}
