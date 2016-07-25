using System;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Ryze.OrbwalkingMode.Jungle
{
    internal class QJungle : FeatureChild<Jungle>
    {
        public override string Name => "[Q] Overload";


        public QJungle(Jungle parent) : base(parent)
        {
            this.OnLoad();
        }

        private void Overload()
        {
            var mobs = MinionManager.GetMinions(Variable.Spells[SpellSlot.E].Range - 20, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (mobs == null) return;

            foreach (var m in mobs)
            {
                Variable.Spells[SpellSlot.Q].Cast(m);
            }
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear || !Variable.Spells[SpellSlot.Q].IsReady() || Variable.Player.IsWindingUp) return;

            if(Menu.Item(Menu.Name + "QMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            this.Overload();
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QMana", "Mana %").SetValue(new Slider(0, 0, 100)));

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
