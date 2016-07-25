using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Gragas.OrbwalkingMode.Lane
{
    internal class LaneQ : FeatureChild<Lane>
    {
        public LaneQ(Lane parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "[Q] Barrel Roll";

        private Logic.QLogic qLogic;

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear || !Variable.Spells[SpellSlot.Q].IsReady()) return;

            if (Menu.Item(Menu.Name + "LaneQMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

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

            var qPred = Variable.Spells[SpellSlot.Q].GetCircularFarmLocation(minions);

            foreach (var m in minions)
            {
                if (qLogic.CanThrowQ())
                {
                   
                    Variable.Spells[SpellSlot.Q].Cast(qPred.Position);
                }

                if (!(Variable.Spells[SpellSlot.Q].GetDamage(m) > m.Health)) continue;

                if (qLogic.CanExplodeQ(m))
                {
                    Variable.Spells[SpellSlot.Q].Cast();
                }
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

        protected override void OnInitialize()
        {
            this.qLogic = new Logic.QLogic();
            base.OnInitialize();
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
