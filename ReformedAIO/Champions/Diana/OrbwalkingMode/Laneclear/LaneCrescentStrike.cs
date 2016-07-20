using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Diana.OrbwalkingMode.Laneclear
{
    class LaneCrescentStrike : FeatureChild<Laneclear>
    {
        public LaneCrescentStrike(Laneclear parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "[Q] Crescent Strike";

        private void OnUpdate(EventArgs args)
        {
            if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear || !Variables.Spells[SpellSlot.Q].IsReady()) return;

            if (Menu.Item(Menu.Name + "LaneQMana").GetValue<Slider>().Value > Variables.Player.ManaPercent) return;

            this.GetMinions();
        }

        private void GetMinions()
        {
            var minions = MinionManager.GetMinions(Menu.Item(Menu.Name + "LaneQDistance").GetValue<Slider>().Value);

            if (minions == null) return;

            if (Menu.Item(Menu.Name + "LaneQEnemy").GetValue<bool>())
            {
                if (minions.Any(m => m.CountEnemiesInRange(1500) > 0))
                {
                    return;
                }
            }

            if (minions.Count < Menu.Item(Menu.Name + "LaneQHit").GetValue<Slider>().Value) return;

            foreach (var qPRed in minions.Select(m => Variables.Spells[SpellSlot.Q].GetPrediction(m)))
            {
                Variables.Spells[SpellSlot.Q].Cast(qPRed.CastPosition);
            }
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "LaneQEnemy", "Only If No Enemies Visible").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "LaneQDistance", "Q Distance")
                .SetValue(new Slider(730, 0, 825)));

            this.Menu.AddItem(new MenuItem(this.Name + "LaneQHit", "Min Minions Hit")
                .SetValue(new Slider(3, 0, 6)));

            this.Menu.AddItem(new MenuItem(this.Name + "LaneQMana", "Mana %").SetValue(new Slider(15, 0, 100)));

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
