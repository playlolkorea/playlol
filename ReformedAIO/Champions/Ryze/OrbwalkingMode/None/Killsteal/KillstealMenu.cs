using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Ryze.Killsteal
{
    internal class KillstealMenu : FeatureChild<Killsteal>
    {
        public KillstealMenu(Killsteal parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "Killsteal Menu";

        private void OnUpdate(EventArgs args)
        {
            var target = HeroManager.Enemies.FirstOrDefault(x => !x.IsDead && x.IsValidTarget(1000));

            if(target == null) return;
           
            if(Variable.Spells[SpellSlot.Q].IsReady() && (Menu.Item(Menu.Name + "KsQ").GetValue<bool>()))
            {
                if (target.Health <= Variable.Spells[SpellSlot.Q].GetDamage(target))
                {
                    Variable.Spells[SpellSlot.Q].Cast(target);
                }
            }

            if (Variable.Spells[SpellSlot.W].IsReady() && (Menu.Item(Menu.Name + "KsW").GetValue<bool>()))
            {
                if (target.Health <= Variable.Spells[SpellSlot.W].GetDamage(target))
                {
                    Variable.Spells[SpellSlot.W].Cast(target);
                }
            }

            if (!Variable.Spells[SpellSlot.E].IsReady() || (!Menu.Item(Menu.Name + "KsE").GetValue<bool>())) return;

            if (target.Health <= Variable.Spells[SpellSlot.E].GetDamage(target))
            {
                Variable.Spells[SpellSlot.E].Cast(target);
            }
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "KsE", "Use E").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "KsW", "Use W").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "KsQ", "Use Q").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
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
