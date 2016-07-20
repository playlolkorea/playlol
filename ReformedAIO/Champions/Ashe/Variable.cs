using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;

namespace ReformedAIO.Champions.Ashe
{
    class Variable
    {
        public static Orbwalking.Orbwalker Orbwalker { get; internal set; }

        public static Obj_AI_Hero Player => ObjectManager.Player;

        public static Dictionary<SpellSlot, Spell> Spells = new Dictionary<SpellSlot, Spell>()
        {
            {
                SpellSlot.Q, new Spell(SpellSlot.Q, 600f)
            },
            {
                SpellSlot.W, new Spell(SpellSlot.W, 1200f)
            },
            {
                SpellSlot.E, new Spell(SpellSlot.E, 2000f)
            },
            {
                 SpellSlot.R, new Spell(SpellSlot.R, 2000f)
            }
        };
    }
}
