namespace ReformedAIO.Champions.Caitlyn.OrbwalkingMode.Combo
{
    using Logic;
    using System;
    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal sealed class WCombo : ChildBase
    {
        public WCombo(string name)
        {
            Name = name;
        }

        public override string Name { get; set; }

        private Obj_AI_Hero Target => TargetSelector.GetTarget(Spells.Spell[SpellSlot.W].Range, TargetSelector.DamageType.Physical);

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            AntiGapcloser.OnEnemyGapcloser -= Gapcloser;
            Events.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            AntiGapcloser.OnEnemyGapcloser += Gapcloser;
            Events.OnUpdate += OnUpdate;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem(Menu.Name + "WMana", "Mana %").SetValue(new Slider(30, 0, 100)));

            Menu.AddItem(new MenuItem(Name + "AntiGapcloser", "Anti-Gapcloser").SetValue(true));

            Menu.AddItem(new MenuItem(Name + "WTarget", "W Behind Target").SetValue(true));

            Menu.AddItem(new MenuItem(Name + "WImmobile", "W On Immobile").SetValue(true));

        //    Menu.AddItem(new MenuItem(Name + "WBush", "Auto W On Bush").SetValue(false));
        }

        private void Gapcloser(ActiveGapcloser gapcloser)
        {
            if (!Menu.Item(Menu.Name + "AntiGapcloser").GetValue<bool>()) return;

            var target = gapcloser.Sender;

            if (target == null) return;

            if (!target.IsEnemy || !Spells.Spell[SpellSlot.W].IsReady()) return;

            Spells.Spell[SpellSlot.W].Cast(gapcloser.End);
        }

        private void OnUpdate(EventArgs args)
        {
            if (Vars.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || Vars.Player.IsWindingUp
                || !Spells.Spell[SpellSlot.W].IsReady()
                || Target == null
                || Menu.Item(Menu.Name + "WMana").GetValue<Slider>().Value > Vars.Player.ManaPercent) return;

            var wPrediction = Spells.Spell[SpellSlot.W].GetPrediction(Target);

            if (Menu.Item(Menu.Name + "WTarget").GetValue<bool>()) 
            {
                if (Utils.TickCount - Spells.Spell[SpellSlot.W].LastCastAttemptT > 3350 && !Spells.Spell[SpellSlot.E].IsReady())
                {
                    Spells.Spell[SpellSlot.W].Cast(wPrediction.CastPosition);
                }

                if (Target.IsInvulnerable || Target.CountEnemiesInRange(1000) < Target.CountAlliesInRange(1000))
                {
                    Spells.Spell[SpellSlot.W].Cast(wPrediction.CastPosition);
                }
            }

            if (wPrediction.Hitchance < HitChance.Immobile || !Menu.Item(Menu.Name + "WImmobile").GetValue<bool>()) return;

            Spells.Spell[SpellSlot.W].Cast(wPrediction.CastPosition);
        }
    }
}
