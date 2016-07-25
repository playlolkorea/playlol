using System;
using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Ryze.Logic;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Ryze.OrbwalkingMode.Mixed
{
    internal class EMixed : FeatureChild<Mixed>
    {
        public EMixed(Mixed parent) : base(parent) { this.OnLoad(); }

        public override string Name => "[E] Spell Flux";

        private ELogic eLogic;

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Mixed || !Variable.Spells[SpellSlot.E].IsReady()) return;

            var target = TargetSelector.GetTarget(this.Menu.Item(Menu.Name + "ERange").GetValue<Slider>().Value, TargetSelector.DamageType.Magical);

            if (target == null) return;

            if (!target.IsValid || !(this.Menu.Item(Menu.Name + "EMana").GetValue<Slider>().Value < Variable.Player.ManaPercent)) return;

            Variable.Spells[SpellSlot.E].Cast(target);
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "ERange", "E Range ").SetValue(new Slider(600, 0, 600)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "EMana", "Mana %").SetValue(new Slider(45, 0, 100)));

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.eLogic = new ELogic();
            base.OnInitialize();
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
