using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Gragas.Logic;
using RethoughtLib.Classes.Feature;
using SharpDX;

namespace ReformedAIO.Champions.Gragas.Menus.Draw
{
    class DrawIndicator : FeatureChild<Draw>
    {
        public DrawIndicator(Draw parent) : base(parent)
        {
            this.OnLoad();
        }

        private HpBarIndicator DrawDamage;

        private LogicAll logic;

        public void OnEndScene(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget(1200) && !ene.IsZombie))
            {
                var EasyKill = Variable.Spells[SpellSlot.R].IsReady()
                       ? new ColorBGRA(0, 255, 0, 120)
                       : new ColorBGRA(255, 255, 0, 120);

                DrawDamage.Unit = enemy;
                DrawDamage.DrawDmg(logic.ComboDmg(enemy), EasyKill);
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
            this.logic = new LogicAll();
            this.DrawDamage = new HpBarIndicator();
            base.OnInitialize();
        }

        protected override void OnDisable()
        {
            Drawing.OnEndScene -= this.OnEndScene;
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
