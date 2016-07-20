using System;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;

namespace ReformedAIO.Champions.Gragas.Menus.Draw
{
    internal class DrawQ : FeatureChild<Draw>
    {
        public DrawQ(Draw parent) : base(parent)
        {
            this.OnLoad();
        }

        public void OnDraw(EventArgs args)
        {
            if (Variable.Player.IsDead) return;

            Render.Circle.DrawCircle(Variable.Player.Position, 825,
                Variable.Spells[SpellSlot.Q].IsReady()
                    ? System.Drawing.Color.FromArgb(120, 0, 170, 255)
                    : System.Drawing.Color.IndianRed);
        }

        protected override sealed void OnLoad()
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

        public override string Name => "Draw [Q]";
    }
}
