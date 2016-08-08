namespace ReformedAIO.Champions.Caitlyn.OrbwalkingMode.Combo
{
    using Logic;
    using System;
    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal sealed class Ewqr : ChildBase
    {
        public Ewqr(string name)
        {
            this.Name = name;
        }

        public override string Name { get; set; }

        private EwqrLogic _ewqrLogic;

        private Obj_AI_Hero Target => TargetSelector.GetTarget(Spells.Spell[SpellSlot.Q].Range, TargetSelector.DamageType.Physical);

        protected override void OnDisable(object sender, Base.FeatureBaseEventArgs featureBaseEventArgs)
        {
            Events.OnUpdate -= this.OnUpdate;
        }

        protected override void OnEnable(object sender, Base.FeatureBaseEventArgs featureBaseEventArgs)
        {
            Events.OnUpdate += this.OnUpdate;
        }

        protected override void OnInitialize(object sender, Base.FeatureBaseEventArgs featureBaseEventArgs)
        {
            this._ewqrLogic = new EwqrLogic();
        }

        private void OnUpdate(EventArgs args)
        {
            if (!Spells.Spell[SpellSlot.Q].IsReady()
                || !Spells.Spell[SpellSlot.E].IsReady()
                || !Spells.Spell[SpellSlot.W].IsReady()
                || Vars.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || _ewqrLogic.EwqrDmg(Target) < Target.Health
                || !_ewqrLogic.EwqrMana())
            {
                return;
            }

            var qePred = Spells.Spell[SpellSlot.E].GetPrediction(Target);
            var wPred = Spells.Spell[SpellSlot.W].GetPrediction(Target);

            Spells.Spell[SpellSlot.E].Cast(qePred.CastPosition);
            Spells.Spell[SpellSlot.W].Cast(wPred.CastPosition);
            Spells.Spell[SpellSlot.Q].Cast(qePred.CastPosition);

            if (Spells.Spell[SpellSlot.R].IsReady() && Target.Health < Spells.Spell[SpellSlot.R].GetDamage(Target))
            {
                Spells.Spell[SpellSlot.R].CastOnUnit(Target);
            }
        }
    }
}
