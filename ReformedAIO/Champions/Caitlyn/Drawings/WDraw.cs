namespace ReformedAIO.Champions.Caitlyn.Drawings
{
    using System;
    using System.Drawing;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using Logic;

    sealed class WDraw : ChildBase
    {
        public WDraw(string name)
        {
            this.Name = name;
        }

        public override string Name { get; set; }

        public void OnDraw(EventArgs args)
        {
            if (Vars.Player.IsDead) return;

            if (this.Menu.Item(this.Menu.Name + "WReady").GetValue<bool>() && !Spells.Spell[SpellSlot.W].IsReady()) return;

            Render.Circle.DrawCircle(
                 Vars.Player.Position,
                Spells.Spell[SpellSlot.W].Range,
                Spells.Spell[SpellSlot.W].IsReady()
                ? Color.White
                : Color.DarkSlateGray
                , Vars.Player.GetSpell(SpellSlot.W).Ammo); // hehe xd
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Drawing.OnDraw -= this.OnDraw;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Drawing.OnDraw += this.OnDraw;
        }

        protected sealed override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            this.Menu.AddItem(new MenuItem(this.Name + "WReady", "Only If Ready").SetValue(false));
        }
    }
}
