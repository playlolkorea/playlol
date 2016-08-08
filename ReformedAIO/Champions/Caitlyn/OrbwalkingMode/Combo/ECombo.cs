namespace ReformedAIO.Champions.Caitlyn.OrbwalkingMode.Combo
{
    using System;
    using LeagueSharp;
    using LeagueSharp.Common;
    using Logic;
    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal sealed class ECombo : ChildBase
    {
        public ECombo(string name)
        {
            this.Name = name;
        }

        public override string Name { get; set; }

        private Obj_AI_Hero Target => TargetSelector.GetTarget(Spells.Spell[SpellSlot.E].Range, TargetSelector.DamageType.Physical);

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            AntiGapcloser.OnEnemyGapcloser -= this.Gapcloser;
            Events.OnUpdate -= this.OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            AntiGapcloser.OnEnemyGapcloser += this.Gapcloser;
            Events.OnUpdate += this.OnUpdate;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "EMana", "Mana %").SetValue(new Slider(30, 0, 100)));

            this.Menu.AddItem(new MenuItem(this.Name + "AntiGapcloser", "Anti Gapcloser").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "AntiMelee", "E Anti-Melee").SetValue(true));
        }

        private void Gapcloser(ActiveGapcloser gapcloser)
        {
            if (!this.Menu.Item(this.Menu.Name + "AntiGapcloser").GetValue<bool>()) return;

            var target = gapcloser.Sender;

            if (target == null) return;

            if (!target.IsEnemy || !Spells.Spell[SpellSlot.E].IsReady()) return;

            Spells.Spell[SpellSlot.E].Cast(gapcloser.End);
        }

        private void OnUpdate(EventArgs args)
        {
            if (Vars.Player.IsWindingUp
                || !Spells.Spell[SpellSlot.E].IsReady()
                || this.Target == null
                || this.Menu.Item(this.Menu.Name + "EMana").GetValue<Slider>().Value > Vars.Player.ManaPercent)
            {
                return;
            }

            if (!Target.IsImmovable && Target.Distance(Vars.Player) > 270 &&
                this.Menu.Item(this.Menu.Name + "AntiMelee").GetValue<bool>())
            {
                return;
            }

            var ePrediction = (Spells.Spell[SpellSlot.E].GetPrediction(this.Target));

            Spells.Spell[SpellSlot.E].Cast(ePrediction.CastPosition);
            //Vars.Player.IsCastingInterruptableSpell(true);

            //if (!Spells.Spell[SpellSlot.Q].IsReady()) return;

            //Spells.Spell[SpellSlot.Q].CastCancelSpell(ePrediction.CastPosition);
        }
    }
}
