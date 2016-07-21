using System;
using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Ashe.Logic;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Ashe.OrbwalkingMode.Combo
{
    class QCombo : FeatureChild<Combo>
    {
        public override string Name => "[Q] Ranger's Focus";

        public QCombo(Combo parent) : base(parent)
        {
            this.OnLoad();
        }

        private QLogic qLogic;

        private void RangersFocus()
        {
            var target = TargetSelector.GetTarget(Orbwalking.GetRealAutoAttackRange(Variable.Player), TargetSelector.DamageType.Physical);
            
            if (target == null || !target.IsValid) return;

            if(Menu.Item(Menu.Name + "AAQ").GetValue<bool>() && Variable.Player.IsWindingUp) return;

            Variable.Spells[SpellSlot.Q].Cast();

            qLogic.Kite(target);
        }

        
        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo || !Variable.Spells[SpellSlot.Q].IsReady()) return;

            if (Menu.Item(Menu.Name + "QMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            this.RangersFocus();
        }

        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QMana", "Mana %").SetValue(new Slider(0, 0, 50))); // You'd have to be retarded to set it over 50%

            this.Menu.AddItem(new MenuItem(this.Name + "AAQ", "AA Before Q").SetValue(true).SetTooltip("AA Q Reset"));

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.qLogic = new Logic.QLogic();
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
