using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Diana.OrbwalkingMode.Jungleclear
{
    internal class JungleCrescentStrike : FeatureChild<Jungleclear>
    {
        public JungleCrescentStrike(Jungleclear parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "[Q] Crescent Strike";

        private void OnUpdate(EventArgs args)
        {
            if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear || !Variables.Spells[SpellSlot.Q].IsReady()) return;

            if (Menu.Item(Menu.Name + "JungleQMana").GetValue<Slider>().Value > Variables.Player.ManaPercent) return;

            this.GetMob();
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "JungleQDistance", "Q Distance")
                .SetValue(new Slider(730, 0, 825)));

            this.Menu.AddItem(new MenuItem(this.Name + "JungleQMana", "Mana %").SetValue(new Slider(15, 0, 100)));

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        private void GetMob()
        {
            var mobs = MinionManager.GetMinions(Menu.Item(Menu.Name + "JungleQDistance").GetValue<Slider>().Value, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if(mobs == null) return;

            foreach (var m in mobs)
            {
                if(!m.IsValid) return;

                Variables.Spells[SpellSlot.Q].Cast(m);
            }
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
