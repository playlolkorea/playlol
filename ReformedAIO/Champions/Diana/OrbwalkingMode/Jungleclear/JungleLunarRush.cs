using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Diana.OrbwalkingMode.Jungleclear
{
    internal class JungleLunarRush : FeatureChild<Jungleclear>
    {
        public JungleLunarRush(Jungleclear parent) : base(parent)
        {
            OnLoad();
        }

        public override string Name => "[W] Lunar Rush";

        private void OnUpdate(EventArgs args)
        {
            if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear || !Variables.Spells[SpellSlot.W].IsReady()) return;

            if (Menu.Item(Menu.Name + "JungleWMana").GetValue<Slider>().Value > Variables.Player.ManaPercent) return;

            this.GetMob();
        }

        private void GetMob()
        {
            var mobs = MinionManager.GetMinions(150 + ObjectManager.Player.AttackRange, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();

            if (mobs == null) return;

            Variables.Spells[SpellSlot.W].Cast(mobs);
        }

        protected sealed override void OnLoad()
        {
            Menu = new Menu(Name, Name);

            Menu.AddItem(new MenuItem(Name + "JungleWMana", "Mana %")
                .SetValue(new Slider(10, 0, 50)));

            Menu.AddItem(new MenuItem(Name + "Enabled", "Enabled").SetValue(true));

            Parent.Menu.AddSubMenu(Menu);
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
