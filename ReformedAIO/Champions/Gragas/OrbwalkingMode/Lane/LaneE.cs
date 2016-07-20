using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Gragas.OrbwalkingMode.Lane
{
    class LaneE : FeatureChild<Lane>
    {

        public LaneE(Lane parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "[E] Body Slam";

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear || !Variable.Spells[SpellSlot.E].IsReady()) return;

            if (Menu.Item(Menu.Name + "LaneEMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            this.GetMinions();
        }

        private void GetMinions()
        {
            var minions = MinionManager.GetMinions(Menu.Item(Menu.Name + "LaneEDistance").GetValue<Slider>().Value);

            if (minions == null) return;

            if (Menu.Item(Menu.Name + "LaneEEnemy").GetValue<bool>())
            {
                if (minions.Any(m => m.CountEnemiesInRange(1500) > 0))
                {
                    return;
                }
            }

            if (minions.Count < Menu.Item(Menu.Name + "LaneEHit").GetValue<Slider>().Value) return;

            var ePred = Variable.Spells[SpellSlot.E].GetCircularFarmLocation(minions);

            Variable.Spells[SpellSlot.E].Cast(ePred.Position);
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "LaneEEnemy", "Only If No Enemies Visible").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "LaneEDistance", "E Distance")
               .SetValue(new Slider(500, 0, 600)));

            this.Menu.AddItem(new MenuItem(this.Name + "LaneEHit", "Min Minions Hit")
                .SetValue(new Slider(3, 0, 6)));

            this.Menu.AddItem(new MenuItem(this.Name + "LaneEMana", "Mana %").SetValue(new Slider(15, 0, 100)));

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
