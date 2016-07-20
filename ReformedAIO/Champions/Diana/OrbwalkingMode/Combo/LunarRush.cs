using System;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Diana.OrbwalkingMode.Combo
{
    class LunarRush : FeatureChild<Combo>
    {
        public LunarRush(Combo parent) : base(parent)
        {
            OnLoad();
        }

        public override string Name => "[W] Lunar Rush";

        private Logic.LogicAll Logic;

        private void lunarrush()
        {
            var target = TargetSelector.GetTarget(Variables.Player.AttackRange + Variables.Player.BoundingRadius, TargetSelector.DamageType.Magical);

            if (target == null || !target.IsValid) return;

            if (Menu.Item(Menu.Name + "WKillable").GetValue<bool>() && Logic.ComboDmg(target) < target.Health)
            {
                return;
            }

            Variables.Spells[SpellSlot.W].Cast();

        }

        private void OnUpdate(EventArgs args)
        {
            if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo || !Variables.Spells[SpellSlot.W].IsReady()) return;

            lunarrush();
        }

        protected sealed override void OnLoad()
        {
            Menu = new Menu(Name, Name);

            Menu.AddItem(new MenuItem(Name + "WKillable", "Use Only If Killable By Combo").SetValue(false));

          //   this.Menu.AddItem(new MenuItem(this.Name + "WMana", "Mana %")
          //       .SetValue(new Slider(15, 100)));
            
            Menu.AddItem(new MenuItem(Name + "Enabled", "Enabled").SetValue(true));
            
            Parent.Menu.AddSubMenu(Menu);
        }

        protected override void OnInitialize()
        {
            this.Logic = new Logic.LogicAll();
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
