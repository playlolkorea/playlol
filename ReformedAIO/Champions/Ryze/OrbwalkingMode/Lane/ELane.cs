using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Ryze.Logic;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Ryze.OrbwalkingMode.Lane
{
    internal class ELane : FeatureChild<Lane>
    {
        public ELane(Lane parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "[E] Spell Flux";

        private ELogic eLogic;

       
        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear || !Variable.Spells[SpellSlot.E].IsReady()) return;

            if (Menu.Item(Menu.Name + "LaneEMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            this.GetMinions();
        }

        private void GetMinions()
        {
            if (Variable.Player.Mana < Variable.Spells[SpellSlot.Q].ManaCost) return;

            var minions = MinionManager.GetMinions(Variable.Spells[SpellSlot.E].Range);

            if (minions == null) return;

            if (Menu.Item(Menu.Name + "LaneEEnemy").GetValue<bool>() && minions.Any(m => m.CountEnemiesInRange(1750) > 0))
            {
                return;
            }

            foreach (var m in minions)
            {
                if (eLogic.RyzeE(m) || m.Health > Variable.Player.GetAutoAttackDamage(m) && m.Health > Variable.Spells[SpellSlot.E].GetDamage(m))
                {
                    Variable.Spells[SpellSlot.E].Cast(m);
                }
            }
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "LaneEEnemy", "Only If No Enemies Visible").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "LaneEMana", "Mana %").SetValue(new Slider(65, 0, 100)));

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.eLogic = new ELogic();
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
