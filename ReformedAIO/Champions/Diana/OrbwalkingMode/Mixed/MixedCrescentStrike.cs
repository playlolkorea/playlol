using System;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Diana.OrbwalkingMode.Mixed
{
    internal class MixedCrescentStrike : FeatureChild<Mixed>
    {
        public MixedCrescentStrike(Mixed parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "[Q] Crescent Strike";

        private Logic.CrescentStrikeLogic qLogic;

        public void OnUpdate(EventArgs args)
        {
            if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Mixed || !Variables.Spells[SpellSlot.Q].IsReady()) return;

            if (Menu.Item(Menu.Name + "QMana").GetValue<Slider>().Value > Variables.Player.ManaPercent) return;

            this.Crescent();
        }

        private void Crescent()
        {
            var target = TargetSelector.GetTarget(Menu.Item(Menu.Name + "QRange").GetValue<Slider>().Value,
                TargetSelector.DamageType.Magical);

            if (target == null || !target.IsValid) return;

            Variables.Spells[SpellSlot.Q].Cast(qLogic.QPred(target));
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QRange", "Q Range ").SetValue(new Slider(820, 0, 825)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QMana", "Mana %").SetValue(new Slider(45, 0, 100)));

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));
            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.qLogic = new Logic.CrescentStrikeLogic();
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
