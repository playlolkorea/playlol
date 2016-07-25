using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Gragas.OrbwalkingMode.Jungle
{
    internal class QJungle : FeatureChild<Jungle>
    {
        public override string Name => "[Q] Barrel Roll";

        private Logic.QLogic qLogic;

        public QJungle(Jungle parent) : base(parent)
        {
            this.OnLoad();
        }


        private void BarrelRoll()
        {
            var mobs = MinionManager.GetMinions(Menu.Item(Menu.Name + "QRange")
                .GetValue<Slider>().Value, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth)
                .FirstOrDefault();

            if(mobs == null || !mobs.IsValid) return;

            Variable.Spells[SpellSlot.Q].Cast(mobs.ServerPosition);

        }

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear || !Variable.Spells[SpellSlot.Q].IsReady()) return;

            if (Menu.Item(Menu.Name + "QMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            this.BarrelRoll();
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QRange", "Q Range ").SetValue(new Slider(835, 0, 850)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QMana", "Mana %").SetValue(new Slider(0, 0, 100)));

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
