using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Ryze.Logic;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Ryze.OrbwalkingMode.Lane
{
    internal class QLane : FeatureChild<Lane>
    {
        public QLane(Lane parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "[Q] Overload";

        private ELogic eLogic;

        private QLogic qLoigc;

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear || !Variable.Spells[SpellSlot.Q].IsReady()) return;

            if (Menu.Item(Menu.Name + "LaneQMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            this.GetMinions();
        }

        private void GetMinions()
        {
            var minions = MinionManager.GetMinions(Variable.Spells[SpellSlot.Q].Range);

            if (minions == null) return;

            if (Menu.Item(Menu.Name + "LaneQEnemy").GetValue<bool>() && minions.Any(m => m.CountEnemiesInRange(1750) > 0))
            {
                return;
            }

            foreach (var m in minions)
            {
                if (eLogic.RyzeE(m))
                {
                    Variable.Spells[SpellSlot.Q].Cast(qLoigc.QPred(m));
                }
            }
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "LaneQEnemy", "Only If No Enemies Visible").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "LaneQMana", "Mana %").SetValue(new Slider(65, 0, 100)));

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.eLogic = new ELogic();
            this.qLoigc = new QLogic();
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
