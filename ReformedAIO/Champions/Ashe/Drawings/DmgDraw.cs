using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Ashe.Logic;
using RethoughtLib.Classes.Feature;
using SharpDX;

namespace ReformedAIO.Champions.Ashe.Drawings
{
    internal class DmgDraw : FeatureChild<Draw>
    {
        public DmgDraw(Draw parent) : base(parent)
        {
            this.OnLoad();
        }

        private HpBarIndicator DrawDamage;

        private RLogic logic; // Cba make new one, might aswell make use of this.

        public void OnDraw(EventArgs args)
        {
            if (Variable.Player.IsDead) return;



            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget(900) && !ene.IsZombie))
            {
                var EasyKill = Variable.Spells[SpellSlot.R].IsReady()
                       ? new ColorBGRA(0, 255, 0, 120)
                       : new ColorBGRA(255, 255, 0, 120);

                DrawDamage.Unit = enemy;
                DrawDamage.DrawDmg(logic.ComboDamage(enemy), EasyKill);
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
            this.logic = new RLogic();
            this.DrawDamage = new HpBarIndicator();
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

        public override string Name => "Draw Damage";
    }
}
