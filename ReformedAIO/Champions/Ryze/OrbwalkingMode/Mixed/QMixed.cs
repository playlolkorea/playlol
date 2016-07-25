using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;
using System;
using LeagueSharp;

namespace ReformedAIO.Champions.Ryze.OrbwalkingMode.Mixed
{
    internal class QMixed : FeatureChild<Mixed>
    {
        public QMixed(Mixed parent) : base(parent) { this.OnLoad(); }

        public override string Name => "[Q] Overload";

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Mixed || !Variable.Spells[SpellSlot.Q].IsReady()) return;

            var target = TargetSelector.GetTarget(this.Menu.Item(Menu.Name + "QRange").GetValue<Slider>().Value, TargetSelector.DamageType.Magical);

            if (target == null) return;

            if (!target.IsValid || !(this.Menu.Item(Menu.Name + "QMana").GetValue<Slider>().Value < Variable.Player.ManaPercent)) return;

            Variable.Spells[SpellSlot.Q].CastIfHitchanceEquals(target, HitChance.High);
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QRange", "Q Range ").SetValue(new Slider(1000, 0, 1000)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QMana", "Mana %").SetValue(new Slider(45, 0, 100)));

           
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

       
        protected override void OnDisable()
        {
            Events.OnUpdate -= this.OnUpdate;
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            Events.OnUpdate += this.OnUpdate;
            base.OnEnable();
        }
    }
}
