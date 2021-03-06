﻿namespace ReformedAIO.Champions.Ryze.OrbwalkingMode.Mixed
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Ryze.Logic;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal class EMixed : ChildBase
    {
        #region Fields

        private ELogic eLogic;

        #endregion

        #region Public Properties

        public override string Name { get; set; } = "[E] Spell Flux";

        #endregion

        #region Methods

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Events.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Events.OnUpdate += OnUpdate;
        }

        protected override void OnInitialize(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            eLogic = new ELogic();
        }

        protected sealed override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Menu.AddItem(new MenuItem(Menu.Name + "ERange", "E Range ").SetValue(new Slider(600, 0, 600)));

            Menu.AddItem(new MenuItem(Menu.Name + "EMana", "Mana %").SetValue(new Slider(45, 0, 100)));
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Mixed
                || !Variable.Spells[SpellSlot.E].IsReady()) return;

            var target = TargetSelector.GetTarget(
                Menu.Item(Menu.Name + "ERange").GetValue<Slider>().Value,
                TargetSelector.DamageType.Magical);

            if (target == null) return;

            if (!target.IsValid
                || !(Menu.Item(Menu.Name + "EMana").GetValue<Slider>().Value < Variable.Player.ManaPercent)) return;

            Variable.Spells[SpellSlot.E].Cast(target);
        }

        #endregion
    }
}