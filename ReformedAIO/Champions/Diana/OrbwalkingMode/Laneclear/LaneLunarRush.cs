using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Diana.OrbwalkingMode.Laneclear
{
    internal class LaneLunarRush : FeatureChild<Laneclear>
    {
        public LaneLunarRush(Laneclear parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "[W] Lunar Rush";

        private void OnUpdate(EventArgs args)
        {
            if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear || !Variables.Spells[SpellSlot.W].IsReady()) return;

            if (Menu.Item(Menu.Name + "LaneWMana").GetValue<Slider>().Value > Variables.Player.ManaPercent) return;

            this.GetMinions();
        }

        private void GetMinions()
        {
            var minions = MinionManager.GetMinions(200f);

            if (minions == null) return;

            if(minions.Count < 3) return;

            if (Menu.Item(Menu.Name + "LaneWEnemy").GetValue<bool>())
            {
                if (minions.Any(m => m.CountEnemiesInRange(1500) > 0))
                {
                    return;
                }
            }

            Variables.Spells[SpellSlot.W].Cast();
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "LaneWEnemy", "Only If No Enemies Visible").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "LaneWMana", "Mana %").SetValue(new Slider(15, 0, 100)));

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
