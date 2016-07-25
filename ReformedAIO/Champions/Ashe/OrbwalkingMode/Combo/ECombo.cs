using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Ashe.Logic;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Ashe.OrbwalkingMode.Combo
{
    internal class ECombo : FeatureChild<Combo>
    {
        public override string Name => "[E] Hawkshot";

        public ECombo(Combo parent) : base(parent)
        {
            this.OnLoad();
        }

        private ELogic eLogic;

        private void OnDraw(EventArgs args)
        {
            if(Variable.Player.IsDead || !Menu.Item(Menu.Name + "VectorDraw").GetValue<bool>()) return;

            var pos = eLogic.Camp.FirstOrDefault(x => x.Value.Distance(Variable.Player.Position) > 1500 && x.Value.Distance(Variable.Player.Position) < 7000);

            Render.Circle.DrawCircle(pos.Value, 100, !pos.Value.IsValid() ? System.Drawing.Color.Red : System.Drawing.Color.Green);
        }

        private void Hawkshot()
        {
            var target = TargetSelector.GetTarget(Variable.Player.AttackRange, TargetSelector.DamageType.Physical);

            if (target == null || !target.IsValid || target.Distance(Variable.Player) > Menu.Item(Menu.Name + "EDistance").GetValue<Slider>().Value || target.IsVisible) return;

            if(!eLogic.ComboE(target)) return;

            foreach (var position in HeroManager.Enemies.Where(x => !x.IsDead && x.Distance(Variable.Player) < 1500))
            {
                var path = position.GetWaypoints().LastOrDefault().To3D();

                if (!NavMesh.IsWallOfGrass(path, 1)) return;

                if (position.Distance(path) > 1500) return;

                if (NavMesh.IsWallOfGrass(Variable.Player.Position, 1)) return; // Stil no proof wether or not this work yet.

                Variable.Spells[SpellSlot.E].Cast(path);
            }
        }

        private void EToCamp()
        {
            var pos = eLogic.Camp.FirstOrDefault(x => x.Value.Distance(Variable.Player.Position) > 1500 && x.Value.Distance(Variable.Player.Position) < 7000);

            if(!pos.Value.IsValid()) return;

            Utility.DelayAction.Add(290, ()=> Variable.Spells[SpellSlot.E].Cast(pos.Value)); // Humanized
        }

        private void OnUpdate(EventArgs args)
        {
            if(!Variable.Spells[SpellSlot.E].IsReady() || Variable.Player.IsRecalling() || Variable.Player.InShop()) return;

            if (Menu.Item(Menu.Name + "ECount").GetValue<bool>() && eLogic.GetEAmmo() == 1) return;

            if (Variable.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.None) // I could make a new class and stuff but, doesn't really matter
            {
                this.EToCamp();
            }

            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo) return;

            this.Hawkshot();
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            
            this.Menu.AddItem(new MenuItem(this.Menu.Name + "EDistance", "Distance").SetValue(new Slider(1500, 0, 1500)).SetTooltip("Only for enemeis & not objectives"));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "ECount", "Save 1 Charge").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "EToVector", "E To Objectives").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "VectorDraw", "Draw Objective Position").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.eLogic = new Logic.ELogic();
            base.OnInitialize();
        }

        protected override void OnDisable()
        {
            Drawing.OnDraw -= this.OnDraw;
            Events.OnUpdate -= this.OnUpdate;
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            Drawing.OnDraw += this.OnDraw;
            Events.OnUpdate += this.OnUpdate;
            base.OnEnable();
        }
    }
}
