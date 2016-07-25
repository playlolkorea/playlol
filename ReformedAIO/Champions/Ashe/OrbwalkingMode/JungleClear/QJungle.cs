using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Ashe.OrbwalkingMode.JungleClear
{
    internal class QJungle : FeatureChild<Jungle>
    {
        public override string Name => "[Q] Ranger's Focus";

        
        public QJungle(Jungle parent) : base(parent)
        {
            this.OnLoad();
        }

        private void RangersFocus()
        {
            var mobs = MinionManager.GetMinions(Variable.Player.AttackRange, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();

            if (mobs == null || !mobs.IsValid) return;

            if(Menu.Item(Menu.Name + "QOverkill").GetValue<bool>() && mobs.Health < Variable.Player.GetAutoAttackDamage(mobs) * 2 && Variable.Player.HealthPercent >= 13) return;

            Variable.Spells[SpellSlot.Q].Cast();
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear || !Variable.Spells[SpellSlot.Q].IsReady() || Variable.Player.IsWindingUp) return;

           
            this.RangersFocus();
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QOverkill", "Overkill Check").SetValue(true));

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
