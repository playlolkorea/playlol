using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Gragas.OrbwalkingMode.Jungle
{
    internal class WJungle : FeatureChild<Jungle>
    {
        public override string Name => "[W] Drunken Rage";

        public WJungle(Jungle parent) : base(parent)
        {
            this.OnLoad();
        }


        private void DrunkenRage()
        {
            var mobs = MinionManager.GetMinions(350, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();

            if (mobs == null || !mobs.IsValid || Variable.Player.IsWindingUp) return;

            if(mobs.HealthPercent < 15) return;

            Variable.Spells[SpellSlot.W].Cast();
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear || !Variable.Spells[SpellSlot.W].IsReady()) return;
            if (Menu.Item(Menu.Name + "WMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            this.DrunkenRage();
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "WMana", "Mana %").SetValue(new Slider(10, 0, 100)));

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
