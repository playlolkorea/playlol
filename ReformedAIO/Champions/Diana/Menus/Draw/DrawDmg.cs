using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using SharpDX;

namespace ReformedAIO.Champions.Diana.Menus.Draw
{
    class DrawDmg : FeatureChild<Draw>
    {
        public DrawDmg(Draw parent) : base(parent)
        {
            this.OnLoad();
        }

        private Logic.LogicAll Logic;

        private HpBarIndicator DrawDamage;

        public void OnEndScene(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget(1200) && !ene.IsZombie))
            {
                var EasyKill = Variables.Spells[SpellSlot.R].IsReady()
                       ? new ColorBGRA(0, 255, 0, 120)
                       : new ColorBGRA(255, 255, 0, 120);

                DrawDamage.Unit = enemy;
                DrawDamage.DrawDmg(Logic.ComboDmg(enemy), EasyKill);
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
            this.Logic = new Logic.LogicAll();
            this.DrawDamage = new HpBarIndicator();
            base.OnInitialize();
        }

        protected override void OnDisable()
        {
            Drawing.OnEndScene-= this.OnEndScene;
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            Drawing.OnEndScene += this.OnEndScene;
            base.OnEnable();
        }

        public override string Name => "Draw Damage";
    }
}
