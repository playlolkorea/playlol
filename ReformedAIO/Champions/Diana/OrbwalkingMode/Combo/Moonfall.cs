using System;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Diana.OrbwalkingMode.Combo
{
    class Moonfall : FeatureChild<Combo>
    {
        public Moonfall(Combo parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "[E] Moonfall";

        private Logic.LogicAll Logic;

        private void OnUpdate(EventArgs args)
        {
            if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo || !Variables.Spells[SpellSlot.E].IsReady()) return;

           // if (Menu.Item(Menu.Name + "EMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            this.moonfall();
        }

        private void moonfall()
        {
            var target = TargetSelector.GetTarget(Menu.Item(Menu.Name + "ERange").GetValue<Slider>().Value,
                TargetSelector.DamageType.Magical);

            if (target == null || !target.IsValid) return;

            if (Menu.Item(Menu.Name + "MinTargets").GetValue<Slider>().Value > target.CountEnemiesInRange(Menu.Item(Menu.Name + "ERange").GetValue<Slider>().Value)) return;

            if (this.Menu.Item(this.Menu.Name + "EKillable").GetValue<bool>() && Logic.ComboDmg(target) * 1.2 < target.Health)
            {
                return;
            }

            Variables.Spells[SpellSlot.E].Cast();
        }

        private void interrupt(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (!this.Menu.Item(this.Menu.Name + "EInterrupt").GetValue<bool>()) return;

            if (!sender.IsEnemy || !Variables.Spells[SpellSlot.E].IsReady() || !sender.IsValidTarget() ||
              sender.IsZombie) return;

            if (sender.IsValidTarget(Variables.Spells[SpellSlot.E].Range)) Variables.Spells[SpellSlot.E].Cast();
        }

        private void gapcloser(ActiveGapcloser gapcloser)
        {
            if (!this.Menu.Item(this.Menu.Name + "EGapcloser").GetValue<bool>()) return;

            var target = gapcloser.Sender;

            if (target == null) return;

            if (!target.IsEnemy || !Variables.Spells[SpellSlot.E].IsReady() || !target.IsValidTarget() || target.IsZombie) return;

            if (target.IsValidTarget(Variables.Spells[SpellSlot.E].Range + Variables.Player.BoundingRadius + target.BoundingRadius)) Variables.Spells[SpellSlot.E].Cast();
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "EInterrupt", "Interrupt").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "EGapcloser", "Anti-Gapcloser").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "EKillable", "Use Only If Killable By Combo").SetValue(true));


            this.Menu.AddItem(new MenuItem(this.Name + "ERange", "E Range")
                .SetValue(new Slider(300, 350)));

            this.Menu.AddItem(new MenuItem(this.Name + "MinTargets", "Min Targets In Range")
                .SetValue(new Slider(2, 0, 5)));

            this.Menu.AddItem(new MenuItem(this.Name + "EMana", "Mana %")
                 .SetValue(new Slider(45, 0, 100)));

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));
            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.Logic = new Logic.LogicAll();
        }

        protected override void OnDisable()
        {
            Interrupter2.OnInterruptableTarget -= interrupt;

            AntiGapcloser.OnEnemyGapcloser -= gapcloser;

            Events.OnUpdate -= this.OnUpdate;
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            Interrupter2.OnInterruptableTarget += interrupt;

            AntiGapcloser.OnEnemyGapcloser += gapcloser;
            
            Events.OnUpdate += this.OnUpdate;
            base.OnEnable();
        }
    }
}
