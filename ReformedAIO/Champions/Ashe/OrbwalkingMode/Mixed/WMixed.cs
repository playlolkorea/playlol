using System;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;


namespace ReformedAIO.Champions.Ashe.OrbwalkingMode.Mixed
{
    class WMixed : FeatureChild<Mixed>
    {
        public override string Name => "[W] Volley";

        public WMixed(Mixed parent) : base(parent)
        {
            this.OnLoad();
        }

        private void Volley()
        {
            var target = TargetSelector.GetTarget(Menu.Item(Menu.Name + "WDistance").GetValue<Slider>().Value, TargetSelector.DamageType.Physical);

            if (target == null || !target.IsValid) return;

            Variable.Spells[SpellSlot.W].CastIfHitchanceEquals(target, HitChance.VeryHigh);
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Mixed || !Variable.Spells[SpellSlot.W].IsReady() || Variable.Player.IsWindingUp) return;

            this.Volley();
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "WDistance", "Max Distance").SetValue(new Slider(1100, 0, 1200)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "WMana", "Mana %").SetValue(new Slider(80, 0, 100)));

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
