namespace ReformedAIO.Champions.Caitlyn.OrbwalkingMode.Combo
{
    using Logic;
    using System;
    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal sealed class WCombo : ChildBase
    {
        public WCombo(string name)
        {
            this.Name = name;
        }

        public override string Name { get; set; }

        private Obj_AI_Hero Target => TargetSelector.GetTarget(Spells.Spell[SpellSlot.W].Range, TargetSelector.DamageType.Physical);

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

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "WMana", "Mana %").SetValue(new Slider(30, 0, 100)));

            this.Menu.AddItem(new MenuItem(this.Name + "AntiGapcloser", "Anti-Gapcloser").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "WTarget", "W Behind Target").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "WImmobile", "W On Immobile").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "WBush", "Auto W On Bush").SetValue(false));
        }

        private void Gapcloser(ActiveGapcloser gapcloser)
        {
            if (!this.Menu.Item(this.Menu.Name + "AntiGapcloser").GetValue<bool>()) return;

            var target = gapcloser.Sender;

            if (target == null) return;

            if (!target.IsEnemy || !Spells.Spell[SpellSlot.W].IsReady()) return;

            Spells.Spell[SpellSlot.W].Cast(gapcloser.End);
        }

        private void OnUpdate(EventArgs args)
        {
            if (Vars.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || Vars.Player.IsWindingUp
                || !Spells.Spell[SpellSlot.W].IsReady()
                || this.Target == null
                || this.Menu.Item(this.Menu.Name + "WMana").GetValue<Slider>().Value > Vars.Player.ManaPercent) return;

            if ((this.Menu.Item(this.Menu.Name + "WTarget").GetValue<bool>() &&
                Vars.Player.GetSpell(SpellSlot.W).Ammo == 3) || Target.CountEnemiesInRange(1000) < Target.CountAlliesInRange(1000))
            {
                Spells.Spell[SpellSlot.W].CastIfWillHit(Target, 1);
            }

            var wPrediction = (Spells.Spell[SpellSlot.W].GetPrediction(this.Target));

            if ((wPrediction.Hitchance >= HitChance.Immobile && this.Menu.Item(this.Menu.Name + "WImmobile").GetValue<bool>()) || Vars.Player.IsCastingInterruptableSpell())
            {
                Spells.Spell[SpellSlot.W].Cast(wPrediction.CastPosition);
            }
        }
    }
}
