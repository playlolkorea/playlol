using System;
using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Ashe.Logic;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Ashe.OrbwalkingMode.Combo
{
    internal class RCombo : FeatureChild<Combo>
    {
        public override string Name => "[R] Crystal Arrow";

        public RCombo(Combo parent) : base(parent)
        {
            this.OnLoad();
        }

        private RLogic rLogic;

        private void interrupt(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (!this.Menu.Item(this.Menu.Name + "Interrupt").GetValue<bool>()) return;

            if (!sender.IsEnemy || !Variable.Spells[SpellSlot.R].IsReady() || sender.IsZombie) return;

            if (sender.IsValidTarget(1200)) Variable.Spells[SpellSlot.R].CastIfHitchanceEquals(sender, HitChance.VeryHigh);
        }

        private void gapcloser(ActiveGapcloser gapcloser)
        {
            if (!this.Menu.Item(this.Menu.Name + "Gapclose").GetValue<bool>()) return;

            var target = gapcloser.Sender;

            if (target == null) return;

            if (!target.IsEnemy || !Variable.Spells[SpellSlot.R].IsReady() || !target.IsValidTarget() || target.IsZombie) return;

            if (target.IsValidTarget(800)) Variable.Spells[SpellSlot.R].CastIfHitchanceEquals(target, HitChance.VeryHigh);
        }

        private void CrystalArrow()
        {
            var target = TargetSelector.GetTarget(Menu.Item(Menu.Name + "RDistance").GetValue<Slider>().Value, TargetSelector.DamageType.Physical, false);

            if (target == null || !target.IsValid || target.IsInvulnerable || target.IsDashing()) return;
          
            if (Menu.Item(Menu.Name + "RSafety").GetValue<bool>() && !rLogic.SafeR(target)) return;

            if (Menu.Item(Menu.Name + "RKillable").GetValue<bool>() && !rLogic.Killable(target)) return;

            Variable.Spells[SpellSlot.R].CastIfHitchanceEquals(target, HitChance.High);
        }

        private void SemiR()
        {
            if(!Menu.Item(Menu.Name + "SemiR").GetValue<KeyBind>().Active) return;

            if(Variable.Player.CountEnemiesInRange(1500) == 0) return;

            var target = TargetSelector.GetSelectedTarget();
            if(target == null) return;

            var pred = Variable.Spells[SpellSlot.R].GetPrediction(target);
            if (pred.Hitchance >= HitChance.High)
            {
                Variable.Spells[SpellSlot.R].Cast(pred.CastPosition);
            }
        }

        private void OnUpdate(EventArgs args)
        {
            if (!Variable.Spells[SpellSlot.R].IsReady()) return;

            this.SemiR();

            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo) return;

            if (Menu.Item(Menu.Name + "RMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            this.CrystalArrow();
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "RDistance", "Max Distance").SetValue(new Slider(1100, 0, 1500)).SetTooltip("Too Much And You Might Not Get The Kill"));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "RMana", "Mana %").SetValue(new Slider(10, 0, 100)));

            this.Menu.AddItem(new MenuItem(this.Name + "SemiR", "Semi-Auto R Key").SetValue(new KeyBind('A', KeyBindType.Press)).SetTooltip("Select Your Target First"));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "RKillable", "Only When Killable").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "RSafety", "Safety Check").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "Interrupt", "Interrupt").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "Gapclose", "Gapcloser").SetValue(true));

            

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.rLogic = new Logic.RLogic();
            base.OnInitialize();
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
