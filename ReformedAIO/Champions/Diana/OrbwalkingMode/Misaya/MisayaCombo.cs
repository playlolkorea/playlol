using System;
using System.Collections.Generic;
using System.Globalization;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;
using RethoughtLib.Menu;
using RethoughtLib.Menu.Presets;

namespace ReformedAIO.Champions.Diana.OrbwalkingMode.Misaya
{
    using Logic;

    class MisayaCombo : FeatureChild<Misaya>
    {
        public MisayaCombo(Misaya parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "Misaya";

        private CrescentStrikeLogic qLogic;

        private LogicAll logic;

        public void OnUpdate(EventArgs args)
        {
            if (!Menu.Item(Menu.Name + "Keybind").GetValue<KeyBind>().Active) return;

            Variables.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

            if (Variables.Spells[SpellSlot.R].IsReady() || Variables.Spells[SpellSlot.Q].IsReady())
            {
                this.PaleCascade();
            }

            if (Variables.Spells[SpellSlot.E].IsReady())
            {
                this.MoonFall();
            }

            if (Variables.Spells[SpellSlot.W].IsReady())
            {
                this.LunarRush();
            }
        }

        private void LunarRush()
        {
            var target = TargetSelector.GetTarget(Variables.Player.AttackRange + Variables.Player.BoundingRadius, TargetSelector.DamageType.Magical);

            if (target == null || !target.IsValid) return;

            Variables.Spells[SpellSlot.W].Cast();
        }

        private void MoonFall()
        {
            var target = TargetSelector.GetTarget(Menu.Item(Menu.Name + "ERange").GetValue<Slider>().Value, TargetSelector.DamageType.Magical);

            if (target == null || !target.IsValid) return;

            if(Menu.Item(Menu.Name + "EKillable").GetValue<bool>() && logic.ComboDmg(target) * 1.3 < target.Health) return;

            Variables.Spells[SpellSlot.E].Cast();
        }

        private void PaleCascade()
        {
            var target = TargetSelector.GetTarget(Menu.Item(Menu.Name + "Range").GetValue<Slider>().Value, TargetSelector.DamageType.Magical);

            if (target == null || !target.IsValid) return;

            if (Variables.Spells[SpellSlot.Q].IsReady() && Variables.Spells[SpellSlot.R].IsReady() && target.Distance(Variables.Player) >= 500)
            {
                Variables.Spells[SpellSlot.R].Cast(target);
            }
            
            Variables.Spells[SpellSlot.Q].Cast(qLogic.QPred(target));
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "Keybind", "Keybind").SetValue(new KeyBind('Z', KeyBindType.Press)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "Range", "Range ").SetValue(new Slider(825, 0, 825)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "UseW", "Use W").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "UseE", "Use E").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "ERange", "E Range").SetValue(new Slider(330, 0, 350)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "EKillable", "Only E If Killable").SetValue(true));
             
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.logic = new LogicAll();
            this.qLogic = new Logic.CrescentStrikeLogic();
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
