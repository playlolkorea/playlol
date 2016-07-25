using System;
using System.Drawing;
using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Diana;
using RethoughtLib.Classes.Feature;

namespace ReformedAIO.Champions.Ryze.Drawings
{
    internal class RDraw : FeatureChild<Draw>
    {
        public RDraw(Draw parent) : base(parent)
        {
            this.OnLoad();
        }

        public void OnDraw(EventArgs args)
        {
            if (Variables.Player.IsDead) return;
            
            Render.Circle.DrawCircle(Variable.Player.Position, Variable.Spells[SpellSlot.R].Level <= 1 ? 1500 : 3000, Color.Aqua);
        }

        protected sealed override void OnLoad()
        {
            Menu = new Menu(Name, Name);

            Menu.AddItem(new MenuItem(Name + "Enabled", "Enabled").SetValue(true));

            Parent.Menu.AddSubMenu(Menu);
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

        public override string Name => "[R] Draw";
    }
}
