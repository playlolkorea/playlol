using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using SharpDX;

namespace ReformedAIO.Champions.Ryze.Drawings
{
    internal class DmgDraw : FeatureChild<Draw>
    {
        public DmgDraw(Draw parent) : base(parent)
        {
            this.OnLoad();
        }

        private Logic.Damage dmg;

        private HpBarIndicator DrawDamage;

        public void OnEndScene(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget(1200) && !ene.IsZombie))
            {
                var EasyKill = Variable.Spells[SpellSlot.R].IsReady()
                       ? new ColorBGRA(0, 255, 0, 120)
                       : new ColorBGRA(255, 255, 0, 120);

                DrawDamage.Unit = enemy;
                DrawDamage.DrawDmg(dmg.ComboDmg(enemy), EasyKill);
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
            this.dmg = new Logic.Damage();
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
