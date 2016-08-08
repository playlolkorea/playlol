using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.FeatureSystem.Abstract_Classes;

namespace ReformedAIO.Champions.Caitlyn.Logic
{
    internal class Spells : ChildBase
    {
        public static Dictionary<SpellSlot, Spell> Spell;

        public override string Name { get; set; } = "Spells";

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Spell = new Dictionary<SpellSlot, Spell>();

            var Q = new Spell(SpellSlot.Q, 1250f);
            Q.SetSkillshot(0.65f, 60f, 2200f, false, SkillshotType.SkillshotLine);

            var W = new Spell(SpellSlot.W, 800f);
            W.SetSkillshot(1.5f, 20f, float.MaxValue, false, SkillshotType.SkillshotCircle);

            var E = new Spell(SpellSlot.E, 950f);
            E.SetSkillshot(0.30f, 70f, 2000f, true, SkillshotType.SkillshotLine);

            var R = new Spell(SpellSlot.R, 3000f); // 2000 + (500 * R.Level) 
            R.SetSkillshot(0.7f, 200f, 1500f, false, SkillshotType.SkillshotCircle);
            

            Spell.Add(Q.Slot, Q);
            Spell.Add(W.Slot, W);
            Spell.Add(E.Slot, E);
            Spell.Add(R.Slot, R);
        }
    }
}
