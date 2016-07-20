using System;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Diana.OrbwalkingMode.Combo
{
    class PaleCascade : FeatureChild<Combo>
    {
        public PaleCascade(Combo parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "[R] Pale Cascade";

      
        private Logic.PaleCascadeLogic rLogic;

        private Logic.LogicAll Logic;

        private void paleCascade()
        {
            var target = TargetSelector.GetTarget(825, TargetSelector.DamageType.Magical);

            if (target == null || !target.IsValid) return;

            if (rLogic.tBuff(target))
            {
                Variables.Spells[SpellSlot.R].Cast(target);
            }
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo || !Variables.Spells[SpellSlot.R].IsReady()) return;

            var target = TargetSelector.GetTarget(825, TargetSelector.DamageType.Magical);

            if (target == null || !target.IsValid) return;

            if (Menu.Item(Menu.Name + "REnemies").GetValue<Slider>().Value >= target.CountEnemiesInRange(1400)) return;

            if (Menu.Item(Menu.Name + "RTurret").GetValue<bool>() && target.UnderTurret()) return;

            if (Menu.Item(Menu.Name + "RKillable").GetValue<bool>() && Logic.ComboDmg(target) < target.Health)
            {
                return;
            }

            this.paleCascade();
        }

        protected sealed override void OnLoad()
        {
           
            Menu = new Menu(Name, Name);

            this.Menu.AddItem(new MenuItem(this.Name + "REnemies", "Don't R Into >= x Enemies")
                .SetValue(new Slider(2, 0, 5)));

            this.Menu.AddItem(new MenuItem(this.Name + "RTurret", "Don't R Into Turret").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "RKillable", "Only If Killable").SetValue(true));

            Menu.AddItem(new MenuItem(Name + "Enabled", "Enabled").SetValue(true));
            Parent.Menu.AddSubMenu(Menu);
        }

        protected override void OnInitialize()
        {
            this.rLogic = new Logic.PaleCascadeLogic();
            this.Logic = new Logic.LogicAll();
            base.OnInitialize();
        }

        protected override void OnDisable()
        {
            Events.OnUpdate -= OnUpdate;
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            Events.OnUpdate += OnUpdate;
            base.OnEnable();
        }
    }
}
