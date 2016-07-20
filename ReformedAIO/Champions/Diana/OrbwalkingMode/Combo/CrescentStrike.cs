using System;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Diana.OrbwalkingMode.Combo
{
    class CrescentStrike : FeatureChild<Combo>
    {
        public CrescentStrike(Combo parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "[Q] Crescent Strike";

        private Logic.LogicAll Logic;

        private Logic.CrescentStrikeLogic qLogic;

        public void OnUpdate(EventArgs args)
        {
            if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo || !Variables.Spells[SpellSlot.Q].IsReady()) return;
            
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
            if (Variables.Spells != null) { Variables.Spells[SpellSlot.Q].SetSkillshot(0.25f, 185, 1600, false, SkillshotType.SkillshotCone); }

            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QRange", "Q Range ").SetValue(new Slider(820, 0, 825)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QMana", "Mana %").SetValue(new Slider(10, 0, 100)));

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            SPrediction.Prediction.Initialize(Menu);
            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.Logic = new Logic.LogicAll();
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
