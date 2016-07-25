using System;
using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Ryze.Logic;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Ryze.OrbwalkingMode.Mixed
{
    internal class WMixed : FeatureChild<Mixed>
    {
        public WMixed(Mixed parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "[W] Rune Prison";

        private ELogic eLogic;

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Mixed || !Variable.Spells[SpellSlot.W].IsReady()) return;

            var target = TargetSelector.GetTarget(this.Menu.Item(Menu.Name + "WRange").GetValue<Slider>().Value, TargetSelector.DamageType.Magical);

            if(target == null) return;

            if (!target.IsValid || eLogic.RyzeE(target) || !(this.Menu.Item(Menu.Name + "WMana").GetValue<Slider>().Value < Variable.Player.ManaPercent)) return;

            Variable.Spells[SpellSlot.W].Cast(target);
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "WRange", "W Range ").SetValue(new Slider(600, 0, 600)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "WMana", "Mana %").SetValue(new Slider(45, 0, 100)));

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
