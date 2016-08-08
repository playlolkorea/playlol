using LeagueSharp.SDK.Utils;

namespace ReformedAIO.Champions.Caitlyn.OrbwalkingMode.Combo
{
    using Logic;
    using System;
    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal sealed class QCombo : ChildBase
    {
        public QCombo(string name)
        {
            Name = name;
        }

        public override string Name { get; set; } 

        private Obj_AI_Hero Target => TargetSelector.GetTarget(Spells.Spell[SpellSlot.Q].Range, TargetSelector.DamageType.Physical);

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Events.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Events.OnUpdate += OnUpdate;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem(Menu.Name + "QMana", "Mana %").SetValue(new Slider(30, 0, 100)));

            Menu.AddItem(new MenuItem(Name + "QImmobile", "Q On Immobile").SetValue(true));

            Menu.AddItem(new MenuItem(Name + "QHit", "Cast if 2 can be hit").SetValue(true));
        }

        private void OnUpdate(EventArgs args)
        {
            if (Vars.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || Vars.Player.IsWindingUp
                || !Spells.Spell[SpellSlot.Q].IsReady()
                || Target == null
                || Menu.Item(Menu.Name + "QMana").GetValue<Slider>().Value > Vars.Player.ManaPercent) return;

            if (Menu.Item(Menu.Name + "QHit").GetValue<bool>())
            {
                Spells.Spell[SpellSlot.Q].CastIfWillHit(Target, 2);
            }

            var qPrediction = (Spells.Spell[SpellSlot.Q].GetPrediction(Target));

            if (Vars.Player.IsCastingInterruptableSpell(true) || !Spells.Spell[SpellSlot.E].IsReady() && qPrediction.Hitchance >= HitChance.VeryHigh)
            {
                Spells.Spell[SpellSlot.Q].Cast(qPrediction.CastPosition);
            }

            if (qPrediction.Hitchance >= HitChance.Immobile && Menu.Item(Menu.Name + "QImmobile").GetValue<bool>())
            {
                Spells.Spell[SpellSlot.Q].Cast(qPrediction.CastPosition);
            }
        }
    }
}
