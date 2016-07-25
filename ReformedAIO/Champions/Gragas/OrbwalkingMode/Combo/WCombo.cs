using System;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Gragas.OrbwalkingMode.Combo
{
    internal class WCombo : FeatureChild<Combo>
    {
        public override string Name => "[W] Drunken Rage";

        public WCombo(Combo parent) : base(parent)
        {
            this.OnLoad();
        }

        private void DrunkenRage()
        {
            var target = TargetSelector.GetTarget(Menu.Item(Menu.Name + "WRange").GetValue<Slider>().Value,
              TargetSelector.DamageType.Magical);

            if (target == null || !target.IsValid) return;

            Variable.Spells[SpellSlot.W].Cast();
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo || !Variable.Spells[SpellSlot.W].IsReady()) return;

            if (Menu.Item(Menu.Name + "WMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            this.DrunkenRage();
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "WRange", "W Range ").SetValue(new Slider(500, 0, 800)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "WMana", "Mana %").SetValue(new Slider(45, 0, 100)));

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
