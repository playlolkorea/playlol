using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Ashe.OrbwalkingMode.JungleClear
{
    internal class WJungle : FeatureChild<Jungle>
    {
        public override string Name => "[W] Volley";

        
        public WJungle(Jungle parent) : base(parent)
        {
            this.OnLoad();
        }

        private void Volley()
        {
            var mobs = MinionManager.GetMinions(Menu.Item(Menu.Name + "WRange")
                .GetValue<Slider>().Value, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth)
                .FirstOrDefault();

            if (mobs == null || !mobs.IsValid) return;

            Variable.Spells[SpellSlot.W].CastIfHitchanceEquals(mobs, HitChance.High);
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear || !Variable.Spells[SpellSlot.W].IsReady()) return;

            if (Menu.Item(Menu.Name + "WMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            this.Volley();
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "WRange", "Q Range ").SetValue(new Slider(600, 0, 700)).SetTooltip("Limited, don't want to W blue doing gromp..."));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "WMana", "Mana %").SetValue(new Slider(7, 0, 100)));

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
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
