using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;

namespace ReformedAIO.Champions.Diana.OrbwalkingMode.Flee
{
    internal class FleeMode : FeatureChild<Flee>
    {
        public FleeMode(Flee parent) : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "Flee";

        private Logic.FleeLogic fleeLogic;

        private void OnUpdate(EventArgs args)
        {
            if(!Menu.Item(Menu.Name + "FleeKey").GetValue<KeyBind>().Active) return;

            var jump = fleeLogic.JumpPos.FirstOrDefault(x => x.Value.Distance(ObjectManager.Player.Position) < 300f && x.Value.Distance(Game.CursorPos) < 700f);

            var monster = MinionManager.GetMinions(900f, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.Health).FirstOrDefault();

            var mobs = MinionManager.GetMinions(900, MinionTypes.All, MinionTeam.NotAlly);

            if (jump.Value.IsValid() && Menu.Item(Menu.Name + "FleeVector").GetValue<bool>())
            {
                Variables.Player.IssueOrder(GameObjectOrder.MoveTo, jump.Value);
               
                foreach (var junglepos in fleeLogic.JunglePos.Where(junglepos => Game.CursorPos.Distance(junglepos) <= 350 && ObjectManager.Player.Position.Distance(junglepos) <= 825 && Variables.Spells[SpellSlot.Q].IsReady() && Variables.Spells[SpellSlot.R].IsReady()))
                {
                    Variables.Spells[SpellSlot.Q].Cast(junglepos);

                    if (monster != null)
                    {
                        Variables.Spells[SpellSlot.R].Cast(monster);
                    }
                }
            }
            else
            {
                Variables.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            }

            foreach (var junglepos in fleeLogic.JunglePos)
            {
                if (Game.CursorPos.Distance(junglepos) <= 350 && ObjectManager.Player.Position.Distance(junglepos) <= 900 && Variables.Spells[SpellSlot.Q].IsReady() && Variables.Spells[SpellSlot.R].IsReady())
                {
                    Variables.Spells[SpellSlot.Q].Cast(junglepos);
                    Variables.Spells[SpellSlot.R].Cast(monster);
                }
                else if (Variables.Spells[SpellSlot.R].IsReady() && !Variables.Spells[SpellSlot.Q].IsReady() && monster.Distance(ObjectManager.Player.Position) > 600f && monster.Distance(Game.CursorPos) <= 350f)
                {
                    Variables.Spells[SpellSlot.R].Cast(monster);
                }
            }

            if (!mobs.Any()) return;

            var mob = mobs.MaxOrDefault(x => x.MaxHealth);

            if (!(mob.Distance(Game.CursorPos) <= 750) || !(mob.Distance(ObjectManager.Player) >= 475)) return;

            if (Variables.Spells[SpellSlot.Q].IsReady() && Variables.Spells[SpellSlot.R].IsReady() && ObjectManager.Player.Mana > Variables.Spells[SpellSlot.R].ManaCost + Variables.Spells[SpellSlot.Q].ManaCost && mob.Health > Variables.Spells[SpellSlot.Q].GetDamage(mob))
            {
                Variables.Spells[SpellSlot.Q].Cast(mob);
                Variables.Spells[SpellSlot.R].Cast(mob);
            }
            else
            {
                if (Variables.Spells[SpellSlot.R].IsReady())
                {
                    Variables.Spells[SpellSlot.R].Cast(mob);
                }
            }
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "FleeKey", "Flee Key").SetValue(new KeyBind('A', KeyBindType.Press)));

            this.Menu.AddItem(new MenuItem(this.Name + "FleeMinion", "Flee To Minions").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "FleeMob", "Flee To Mobs").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "FleeVector", "Flee To Vector").SetValue(true).SetTooltip("Flee's To Jungle Camps"));

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));
            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.fleeLogic = new Logic.FleeLogic();
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
