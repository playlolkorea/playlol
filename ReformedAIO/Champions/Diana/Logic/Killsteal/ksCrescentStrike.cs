using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Diana.Logic.Killsteal
{
    class ksCrescentStrike : FeatureChild<Killsteal>
    {
        public ksCrescentStrike(Killsteal parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "[Q] Crescent Strike";

        private CrescentStrikeLogic qLogic;

        private void OnUpdate(EventArgs args)
        {
            var target = HeroManager.Enemies .FirstOrDefault(x => !x.IsDead && x.IsValidTarget(Menu.Item(Menu.Name + "QRange").GetValue<Slider>().Value));

            if(Menu.Item(Menu.Name + "QMana").GetValue<Slider>().Value > Variables.Player.ManaPercent) return;

            if (target != null && target.Health < qLogic.GetDmg(target))
            {
                Variables.Spells[SpellSlot.Q].Cast(qLogic.QPred(target));
            }
        }

        protected override void OnLoad() // TODO Add Dmg Multiplier(?)
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QRange", "Q Range ").SetValue(new Slider(820, 0, 825)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QMana", "Mana %").SetValue(new Slider(45, 0, 100)));

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));
            this.Parent.Menu.AddSubMenu(this.Menu);
        }
        protected override void OnInitialize()
        {
            this.qLogic = new CrescentStrikeLogic();
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
