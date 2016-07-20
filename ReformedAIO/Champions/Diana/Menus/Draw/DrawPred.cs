using System;
using System.Drawing;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;

namespace ReformedAIO.Champions.Diana.Menus.Draw
{
    class DrawPred : FeatureChild<Draw>
    {
        public DrawPred(Draw parent) : base(parent)
        {
            this.OnLoad();
        }

        public void OnDraw(EventArgs args)
        {
            if(Variables.Player.IsDead) return;

            var target = TargetSelector.GetTarget(825, TargetSelector.DamageType.Magical);

            if (target != null && target.IsVisible)
            {
                Render.Circle.DrawCircle(qLogic.QPred(target), 50, Color.Aqua);
            }
        }

        protected sealed override void OnLoad()
        {
            Menu = new Menu(Name, Name);


            Menu.AddItem(new MenuItem(Name + "Enabled", "Enabled").SetValue(true));

            Parent.Menu.AddSubMenu(Menu);
        }

        protected override void OnInitialize()
        {
            this.qLogic = new Logic.CrescentStrikeLogic();
            base.OnInitialize();
        }

        protected override void OnDisable()
        {
            Drawing.OnDraw -= this.OnDraw;
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            Drawing.OnDraw += this.OnDraw;
            base.OnEnable();
        }

        private Logic.CrescentStrikeLogic qLogic;

        public override string Name => "Draw Prediction";
    }
}
