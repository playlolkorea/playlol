using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Diana.Logic.Killsteal
{
    internal class ksPaleCascade : FeatureChild<Killsteal>
    {
        public ksPaleCascade(Killsteal parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "[R] PaleCascade";

        private PaleCascadeLogic rLogic;

        private void OnUpdate(EventArgs args)
        {
            var target = HeroManager.Enemies.FirstOrDefault(x => !x.IsDead && x.IsValidTarget(Menu.Item(Menu.Name + "RRange").GetValue<Slider>().Value));

            if (target == null || target.Health > rLogic.GetDmg(target)) return;

            Variables.Spells[SpellSlot.R].Cast(target);
        }

        protected override void OnLoad() // TODO Add Dmg Multiplier(?)
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "RRange", "R Range ").SetValue(new Slider(825, 0, 825)));

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));
            this.Parent.Menu.AddSubMenu(this.Menu);
        }
        protected override void OnInitialize()
        {
            this.rLogic = new PaleCascadeLogic();
            base.OnInitialize();
        }

        protected override void OnDisable()
        {
            Events.OnUpdate -= this.OnUpdate;
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            Events.OnUpdate += this.OnUpdate;
            base.OnEnable();
        }
    }
}
