using System;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;

namespace ReformedAIO.Champions.Ashe.Drawings
{
    internal class WDraw : FeatureChild<Draw>
    {
        public WDraw(Draw parent) : base(parent)
        {
            this.OnLoad();
        }

        public void OnDraw(EventArgs args)
        {
            if (Variable.Player.IsDead) return;

            if(Menu.Item(Menu.Name + "WReady").GetValue<bool>() && !Variable.Spells[SpellSlot.W].IsReady()) return;

            Render.Circle.DrawCircle(Variable.Player.Position, 1275, Variable.Spells[SpellSlot.W].IsReady()
                    ? System.Drawing.Color.FromArgb(120, 0, 170, 255)
                    : System.Drawing.Color.IndianRed);
        }

        protected override sealed void OnLoad()
        {
            Menu = new Menu(Name, Name);

            Menu.AddItem(new MenuItem(Name + "WReady", "Only If Ready").SetValue(false));

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

        public override string Name => "[W] Draw";
    }
}
