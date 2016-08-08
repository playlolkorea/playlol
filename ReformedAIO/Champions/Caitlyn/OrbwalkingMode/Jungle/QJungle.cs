using ReformedAIO.Champions.Caitlyn.Logic;

namespace ReformedAIO.Champions.Caitlyn.OrbwalkingMode.Jungle
{
    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal class QJungle : ChildBase
    {
        public QJungle(string name)
        {
            this.Name = name;
        }

        public sealed override string Name { get; set; }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Events.OnUpdate -= this.OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Events.OnUpdate += this.OnUpdate;
        }

        protected sealed override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QOverkill", "Overkill Check").SetValue(true));
        }

        private void OnUpdate(EventArgs args)
        {
            if (Vars.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear
                || !Spells.Spell[SpellSlot.Q].IsReady()
                || Vars.Player.IsWindingUp)
            {
                return;
            }

            var mobs = MinionManager.GetMinions(Spells.Spell[SpellSlot.E].Range, MinionTypes.All, MinionTeam.Neutral,MinionOrderTypes.MaxHealth).FirstOrDefault();

            if (mobs == null || !mobs.IsValid) return;

            var qPrediction = (Spells.Spell[SpellSlot.Q].GetPrediction(mobs));

            if (this.Menu.Item(this.Menu.Name + "QOverkill").GetValue<bool>() &&
                mobs.Health < Vars.Player.GetAutoAttackDamage(mobs) * 2) return;

            Spells.Spell[SpellSlot.Q].Cast(qPrediction.CastPosition);
        }
    }
}
