using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Diana.OrbwalkingMode.Jungleclear
{
    class JungleMoonfall : FeatureChild<Jungleclear>
    {
        public JungleMoonfall(Jungleclear parent) : base(parent)
        {
            this.OnLoad();
        }
        public override string Name => "[E] Moonfall";

        private void OnUpdate(EventArgs args)
        {
            if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear || !Variables.Spells[SpellSlot.E].IsReady()) return;

            if (Menu.Item(Menu.Name + "JungleEMana").GetValue<Slider>().Value > Variables.Player.ManaPercent || Menu.Item(Menu.Name + "JungleEHealth")
                .GetValue<Slider>().Value > Variables.Player.HealthPercent) return;

            this.GetMob();

        }

        private void GetMob()
        {
            var mobs = MinionManager.GetMinions(800 + ObjectManager.Player.AttackRange, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();

            if (mobs == null) return;

            Variables.Spells[SpellSlot.E].Cast(mobs);
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "JungleEHealth", "Health %")
                .SetValue(new Slider(15, 0, 35)));

            this.Menu.AddItem(new MenuItem(this.Name + "JungleEMana", "Mana %")
                .SetValue(new Slider(15, 0, 35)));

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(false));
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
