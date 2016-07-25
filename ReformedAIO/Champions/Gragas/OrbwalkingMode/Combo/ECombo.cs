using System;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;
using SPrediction;

namespace ReformedAIO.Champions.Gragas.OrbwalkingMode.Combo
{
    internal class ECombo : FeatureChild<Combo>
    {
        public override string Name => "[E] Body Slam";

        public ECombo(Combo parent) : base(parent)
        {
            this.OnLoad();
        }

        private Logic.LogicAll logic;

        private void BodySlam()
        {
            var target = TargetSelector.GetTarget(Menu.Item(Menu.Name + "ERange").GetValue<Slider>().Value,
              TargetSelector.DamageType.Magical);

            if (target == null || !target.IsValid) return;

            if(Menu.Item(Menu.Name + "EKillable").GetValue<bool>() && logic.ComboDmg(target) < target.Health) return;

            var ePred = Variable.Spells[SpellSlot.E].GetSPrediction(target);

            if(target.HasBuffOfType(BuffType.Knockback)) return;

            if (ePred.HitChance < HitChance.Medium) return;

            Variable.Spells[SpellSlot.E].Cast(ePred.CastPosition);
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo || !Variable.Spells[SpellSlot.E].IsReady()) return;

            if (Menu.Item(Menu.Name + "EMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            this.BodySlam();
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "EKillable", "Only If Killable").SetValue(false));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "ERange", "E Range ").SetValue(new Slider(835, 0, 850)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "EMana", "Mana %").SetValue(new Slider(45, 0, 100)));

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.logic = new Logic.LogicAll();
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
