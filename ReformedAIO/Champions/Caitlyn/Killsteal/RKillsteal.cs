using LeagueSharp.SDK;
using LeagueSharp.SDK.Utils;

namespace ReformedAIO.Champions.Caitlyn.Killsteal
{
    using Logic;
    using System;
    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal sealed class RKillsteal : ChildBase
    {
        public RKillsteal(string name)
        {
            this.Name = name;
        }

        public override string Name { get; set; }

        private Obj_AI_Hero Target => TargetSelector.GetTarget(Spells.Spell[SpellSlot.Q].Range, TargetSelector.DamageType.Physical);

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Events.OnUpdate -= this.OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Events.OnUpdate += this.OnUpdate;
        }

        private void OnUpdate(EventArgs args)
        {
            if (Vars.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None
                || Vars.Player.IsWindingUp
                || !Spells.Spell[SpellSlot.R].IsReady()
                || this.Target == null
                || this.Target.Health > Spells.Spell[SpellSlot.R].GetDamage(this.Target)
                || Vars.Player.IsUnderEnemyTurret())
            {
                return;
            }

            if (this.Target.Distance(Vars.Player) < Vars.Player.GetRealAutoAttackRange())
            {
                return;
            }

            var pos = Spells.Spell[SpellSlot.R].GetPrediction(this.Target);

            if (pos.AoeTargetsHit.Count > 1)// Todo: This just a TEST!
            {
                return;
            } 

            //if (Vars.Player.CountEnemiesInRange(Vars.Player.GetRealAutoAttackRange() + 400) != 0)
            //{
            //    return;
            //}

            Spells.Spell[SpellSlot.R].Cast(pos.CastPosition);
        }
    }
}
