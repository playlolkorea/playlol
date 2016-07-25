using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Diana;
using RethoughtLib.Classes.Feature;

namespace ReformedAIO.Champions.Ryze.Drawings
{
    internal class EDraw : FeatureChild<Draw>
    {
        public EDraw(Draw parent) : base(parent)
        {
            this.OnLoad();
        }

        public void OnDraw(EventArgs args)
        {
            if (Variables.Player.IsDead) return;

            Render.Circle.DrawCircle(Variables.Player.Position, Variables.Spells[SpellSlot.E].Range,
                   Variables.Spells[SpellSlot.E].IsReady() ? System.Drawing.Color.FromArgb(120, 0, 170, 255) : System.Drawing.Color.IndianRed);
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

        public override string Name => "[E] Draw";
    }
}
