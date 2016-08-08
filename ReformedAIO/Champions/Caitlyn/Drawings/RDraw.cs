namespace ReformedAIO.Champions.Caitlyn.Drawings
{
    using System;
    using System.Drawing;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using Logic;

    sealed class RDraw : ChildBase
    {
        public RDraw(string name)
        {
            this.Name = name;
        }

        public override string Name { get; set; }

        public void OnDraw(EventArgs args)
        {
            if (Vars.Player.IsDead) return;

            if (this.Menu.Item(this.Menu.Name + "RReady").GetValue<bool>() && !Spells.Spell[SpellSlot.R].IsReady()) return;

            Render.Circle.DrawCircle(
                 Vars.Player.Position,
                Spells.Spell[SpellSlot.R].Range,
                Spells.Spell[SpellSlot.R].IsReady()
                ? Color.White
                : Color.DarkSlateGray
                , 1);
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Drawing.OnDraw -= this.OnDraw;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Drawing.OnDraw += this.OnDraw;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            this.Menu.AddItem(new MenuItem(this.Name + "RReady", "Only If Ready").SetValue(false));
        }
    }
}
