using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Diana.OrbwalkingMode.Jungleclear
{
    internal class JunglePaleCascade : FeatureChild<Jungleclear>
    {
        public JunglePaleCascade(Jungleclear parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "[R] Pale Cascade";

        private void OnUpdate(EventArgs args)
        {
            if(Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear || !Variables.Spells[SpellSlot.R].IsReady()) return;

            if (Menu.Item(Menu.Name + "JungleRMana").GetValue<Slider>().Value > Variables.Player.ManaPercent) return;

            this.GetMob();
        }

        private void GetMob()
        {
            var mobs = MinionManager.GetMinions(825, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();

            if (mobs == null) return;

            if(!mobs.HasBuff("dianamoonlight")) return;

            Variables.Spells[SpellSlot.R].Cast(mobs);
        }

        protected sealed override void OnLoad()
        {

            Menu = new Menu(Name, Name);

            this.Menu.AddItem(new MenuItem(this.Name + "JungleRMana", "Mana %")
                .SetValue(new Slider(35, 0, 100)));

            Menu.AddItem(new MenuItem(Name + "Enabled", "Enabled").SetValue(true).SetTooltip("Wont cast unless Reset avaible"));
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
