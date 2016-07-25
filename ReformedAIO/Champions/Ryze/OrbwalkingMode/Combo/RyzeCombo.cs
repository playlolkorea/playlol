using System;
using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Ryze.Logic;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Ryze.OrbwalkingMode.Combo
{
    internal class RyzeCombo : FeatureChild<Combo> // Best if i don't split each spell this time. :^) (This should be paid ffs)
    {
        public override string Name => "Combo Menu";

        public RyzeCombo(Combo parent) : base(parent)
        {
            this.OnLoad();
        }

        
        private ELogic eLogic;

     //   protected List<Obj_AI_Hero> Targets;

        private void Burst()
        {                                     // 1000 would be overkill
            var target = TargetSelector.GetTarget(975, TargetSelector.DamageType.Magical);

            if (target == null) return;

            if (Variable.Spells[SpellSlot.Q].IsReady())
            {
                if (target.IsValid && this.Menu.Item(Menu.Name + "QMana").GetValue<Slider>().Value < Variable.Player.ManaPercent)
                {
                    var qpred = Variable.Spells[SpellSlot.Q].GetPrediction(target);
                    if (qpred.Hitchance >= HitChance.Medium)
                    {
                        Variable.Spells[SpellSlot.Q].Cast(qpred.CastPosition);
                    }   
                }
            }

            if (Variable.Spells[SpellSlot.E].IsReady() && !Variable.Spells[SpellSlot.Q].IsReady())
            {
                if (target.IsValidTarget(Variable.Spells[SpellSlot.E].Range) &&
                    this.Menu.Item(Menu.Name + "EMana").GetValue<Slider>().Value < Variable.Player.ManaPercent)
                {
                    Variable.Spells[SpellSlot.E].Cast(target);
                }
            }

            if (!Variable.Spells[SpellSlot.W].IsReady() || eLogic.RyzeE(target)) return;

            if (!target.IsValidTarget(Variable.Spells[SpellSlot.W].Range) ||
                !(this.Menu.Item(Menu.Name + "WMana").GetValue<Slider>().Value < Variable.Player.ManaPercent)) return;

            Variable.Spells[SpellSlot.W].Cast(target);
        }

        private void Safe()
        {
            var target = TargetSelector.GetTarget(1000, TargetSelector.DamageType.Magical);

            if (target == null) return;

            if (Variable.Spells[SpellSlot.E].IsReady())
            {
                Variable.Spells[SpellSlot.E].Cast(target);
            }

            if (eLogic.RyzeE(target) && Variable.Spells[SpellSlot.W].IsReady() &&
                target.IsValidTarget(Variable.Spells[SpellSlot.W].Range))
            {
                Variable.Spells[SpellSlot.W].Cast(target);
            }

            if (Variable.Spells[SpellSlot.Q].IsReady() && !eLogic.RyzeE(target))
            {
                Variable.Spells[SpellSlot.Q].CastIfHitchanceEquals(target, HitChance.High);
            }
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo) return;

            //this.GetTargets();

            switch (this.Menu.Item(Menu.Name + "Mode").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                {
                    this.Burst();
                    break;
                }
                case 1:
                {
                    this.Safe();
                    break;
                }
                case 2: // We could make slider for when to do it in burst, we'll see. That way we don't need to declare new variables
                {
                    var target = TargetSelector.GetTarget(600, TargetSelector.DamageType.Magical);

                    if (target == null) return;

                    if (Variable.Player.HealthPercent <= 15 && target.HealthPercent >= 20)
                    {
                        goto case 1;
                    }
                    goto case 0;
                }
            }
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "Mode", "Mode").SetValue(new StringList(new [] {"Burst", "Safe", "Automatic" }))); // Todo: Dynamic Menu, also automatic mode(?)
           
            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QMana", "Q Mana %").SetValue(new Slider(0, 0, 50))); // Todo: Dynamic Menu, instead of slider use calculations.

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "WMana", "W Mana %").SetValue(new Slider(0, 0, 50)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "EMana", "E Mana %").SetValue(new Slider(0, 0, 50)));

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        //private void GetTargets() // Just a test
        //{
        //    this.Targets = HeroManager.Enemies.Where(x => x.IsValid && x.Distance(GlobalVariables.Player.ServerPosition) <= 1000).ToList();
        //}

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
