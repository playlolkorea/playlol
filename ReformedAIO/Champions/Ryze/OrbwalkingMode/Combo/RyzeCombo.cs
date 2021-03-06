﻿namespace ReformedAIO.Champions.Ryze.OrbwalkingMode.Combo
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Ryze.Logic;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal class RyzeCombo : ChildBase
    {
        #region Fields

        private ELogic eLogic;

        #endregion

        #region Public Properties

        public sealed override string Name { get; set; }

        #endregion

        #region Methods

        public RyzeCombo(string name)
        {
            Name = name;
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += OnUpdate;
        }

        protected override void OnInitialize(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            eLogic = new ELogic();

            base.OnInitialize(sender, featureBaseEventArgs);
        }

        protected sealed override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            //base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(
                new MenuItem(Name + "Mode", "Mode").SetValue(
                    new StringList(new[] { "Burst", "Safe", "Automatic" })));

            Menu.AddItem(new MenuItem(Menu.Name + "QMana", "Q Mana %").SetValue(new Slider(0, 0, 50)));

            Menu.AddItem(new MenuItem(Menu.Name + "WMana", "W Mana %").SetValue(new Slider(0, 0, 50)));

            Menu.AddItem(new MenuItem(Menu.Name + "EMana", "E Mana %").SetValue(new Slider(0, 0, 50)));
        }

        private void Burst()
        {
            var target = TargetSelector.GetTarget(975, TargetSelector.DamageType.Magical);

            if (target == null) return;

            if (Variable.Spells[SpellSlot.Q].IsReady())
            {
                if (target.IsValid
                    && Menu.Item(Menu.Name + "QMana").GetValue<Slider>().Value < Variable.Player.ManaPercent)
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
                if (target.IsValidTarget(Variable.Spells[SpellSlot.E].Range)
                    && Menu.Item(Menu.Name + "EMana").GetValue<Slider>().Value < Variable.Player.ManaPercent)
                {
                    Variable.Spells[SpellSlot.E].Cast(target);
                }
            }

            if (!Variable.Spells[SpellSlot.W].IsReady() || eLogic.RyzeE(target)) return;

            if (!target.IsValidTarget(Variable.Spells[SpellSlot.W].Range)
                || !(Menu.Item(Menu.Name + "WMana").GetValue<Slider>().Value < Variable.Player.ManaPercent)) return;

            Variable.Spells[SpellSlot.W].Cast(target);
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo) return;

            switch (Menu.Item(Menu.Name + "Mode").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    {
                        Burst();
                        break;
                    }
                case 1:
                    {
                        Safe();
                        break;
                    }
                case 2:
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

        private void Safe()
        {
            var target = TargetSelector.GetTarget(1000, TargetSelector.DamageType.Magical);

            if (target == null) return;

            if (Variable.Spells[SpellSlot.E].IsReady())
            {
                Variable.Spells[SpellSlot.E].Cast(target);
            }

            if (eLogic.RyzeE(target) && Variable.Spells[SpellSlot.W].IsReady()
                && target.IsValidTarget(Variable.Spells[SpellSlot.W].Range))
            {
                Variable.Spells[SpellSlot.W].Cast(target);
            }

            if (Variable.Spells[SpellSlot.Q].IsReady() && !eLogic.RyzeE(target))
            {
                Variable.Spells[SpellSlot.Q].CastIfHitchanceEquals(target, HitChance.High);
            }
        }

        #endregion
    }
}