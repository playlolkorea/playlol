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
            this.Name = name;
        }

        public override string Name { get; set; } 

        private Obj_AI_Hero Target => TargetSelector.GetTarget(Spells.Spell[SpellSlot.Q].Range, TargetSelector.DamageType.Physical);

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Events.OnUpdate -= this.OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Events.OnUpdate += this.OnUpdate;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QMana", "Mana %").SetValue(new Slider(30, 0, 100)));

            this.Menu.AddItem(new MenuItem(this.Name + "QImmobile", "Q On Immobile").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "QHit", "Cast if 2 can be hit").SetValue(true));
        }

        private void OnUpdate(EventArgs args)
        {
            if (Vars.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || Vars.Player.IsWindingUp
                || !Spells.Spell[SpellSlot.Q].IsReady()
                || this.Target == null
                || this.Menu.Item(this.Menu.Name + "QMana").GetValue<Slider>().Value > Vars.Player.ManaPercent) return;


            var qPrediction = (Spells.Spell[SpellSlot.Q].GetPrediction(this.Target));

            if (this.Menu.Item(this.Menu.Name + "QHit").GetValue<bool>())
            {
                Spells.Spell[SpellSlot.Q].CastIfWillHit(Target, 2);
            }

            if (qPrediction.Hitchance >= HitChance.Immobile && this.Menu.Item(this.Menu.Name + "QImmobile").GetValue<bool>())
            {
                Spells.Spell[SpellSlot.Q].Cast(qPrediction.CastPosition);
            }
            if (Vars.Player.IsCastingInterruptableSpell())
            {
                Spells.Spell[SpellSlot.Q].Cast(qPrediction.CastPosition);
            }
        }
    }
}
