using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;


namespace ReformedAIO.Champions.Ashe.OrbwalkingMode.LaneClear
{
    internal class WLane : FeatureChild<Lane>
    {
        public WLane(Lane parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "[W] Volley";

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear || !Variable.Spells[SpellSlot.W].IsReady()) return;

            if (Menu.Item(Menu.Name + "LaneWMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            this.GetMinions();
        }

        private void GetMinions()
        {
            var minions = MinionManager.GetMinions(Menu.Item(Menu.Name + "LaneWMDistance").GetValue<Slider>().Value);

            if (minions == null) return;

            if (Menu.Item(Menu.Name + "LaneWEnemy").GetValue<bool>() && minions.Any(m => m.CountEnemiesInRange(2000) > 0))
            {
                return;
            }

            if(minions.Count < 2) return;

            foreach (var m in minions)
            {
                if(m.Health < Variable.Spells[SpellSlot.W].GetDamage(m) * 1.2) return;

                Variable.Spells[SpellSlot.W].CastIfHitchanceEquals(m, HitChance.High);
            }
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "LaneWEnemy", "Only If No Enemies Visible").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "LaneWMDistance", "Distance").SetValue(new Slider(600, 0, 900)).SetTooltip("Put it too high and you'll miss minions"));

            this.Menu.AddItem(new MenuItem(this.Name + "LaneWMana", "Mana %").SetValue(new Slider(70, 0, 100)));

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
