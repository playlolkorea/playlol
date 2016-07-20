using System;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Gragas.OrbwalkingMode.Combo
{
    class QCombo : FeatureChild<Combo>
    {
        public override string Name => "[Q] Barrel Roll";

        private Logic.QLogic qLogic;

        public QCombo(Combo parent) : base(parent)
        {
            this.OnLoad();
        }

        private void BarrelRoll()
        {
            var target = TargetSelector.GetTarget(Menu.Item(Menu.Name + "QRange").GetValue<Slider>().Value,
              TargetSelector.DamageType.Magical);

            if (target == null || !target.IsValid) return;

            if (target.HasBuffOfType(BuffType.Knockback)) return;

            if (qLogic.CanThrowQ())
            {
                Variable.Spells[SpellSlot.Q].Cast(qLogic.QPred(target));
            }
            
            if (qLogic.CanExplodeQ(target))
            {
                Variable.Spells[SpellSlot.Q].Cast();
            }
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo || !Variable.Spells[SpellSlot.Q].IsReady()) return;

            if (Menu.Item(Menu.Name + "QMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            this.BarrelRoll();
        }

        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QRange", "Q Range ").SetValue(new Slider(835, 0, 850)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QMana", "Mana %").SetValue(new Slider(45, 0, 100)));

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
