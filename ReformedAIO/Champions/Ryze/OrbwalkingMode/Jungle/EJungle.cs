using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Ryze.OrbwalkingMode.Jungle
{
    internal class EJungle : FeatureChild<Jungle>
    {
        public override string Name => "[E] Spell Flux";

        public EJungle(Jungle parent) : base(parent)
        {
            this.OnLoad();
        }

        private void SpellFlux()
        {
            var mobs = MinionManager.GetMinions(Variable.Spells[SpellSlot.E].Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (mobs == null) return;

            foreach (var m in mobs)
            {
                Variable.Spells[SpellSlot.E].Cast(m);
            }
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear || !Variable.Spells[SpellSlot.E].IsReady()) return;

            if (Menu.Item(Menu.Name + "EMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            this.SpellFlux();
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "EMana", "Mana %").SetValue(new Slider(0, 0, 100)));

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
