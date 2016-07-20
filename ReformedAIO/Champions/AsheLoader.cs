using System;
using System.Collections.Generic;
using LeagueSharp.Common;
using ReformedAIO.Champions.Ashe;
using ReformedAIO.Champions.Ashe.Drawings;
using ReformedAIO.Champions.Ashe.Logic;
using ReformedAIO.Champions.Ashe.OrbwalkingMode.Combo;
using ReformedAIO.Champions.Ashe.OrbwalkingMode.JungleClear;
using ReformedAIO.Champions.Ashe.OrbwalkingMode.LaneClear;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Classes.Intefaces;
using RethoughtLib.Utility;
using Draw = ReformedAIO.Champions.Ashe.Drawings.Draw; 

namespace ReformedAIO.Champions
{
    class AsheLoader : ILoadable
    {
        private readonly List<IFeatureChild> _apply = new List<IFeatureChild>();

        public string Name { get; set; } = "Ashe";

        public void Load()
        {
            Console.WriteLine("ReformedAIO Loading...");

            var rootMenu = new Menu("Ashe", "Ashe", true);
            rootMenu.AddToMainMenu();

            var orbWalkingMenu = new Menu("Orbwalker", "Orbwalking");
            Variable.Orbwalker = new Orbwalking.Orbwalker(orbWalkingMenu);
            rootMenu.AddSubMenu(orbWalkingMenu);

            var setSpell = new SetSpells();
            setSpell.Load();

            var combo = new Combo(rootMenu);
            var jungle = new Jungle(rootMenu);
            var lane = new Lane(rootMenu);
            var draw = new Draw(rootMenu);

            _apply.Add(new QCombo(combo));
            _apply.Add(new WCombo(combo));
            _apply.Add(new ECombo(combo));
            _apply.Add(new RCombo(combo));

            _apply.Add(new QJungle(jungle));
            _apply.Add(new WJungle(jungle));

            _apply.Add(new QLane(lane));
            _apply.Add(new WLane(lane));

            _apply.Add(new WDraw(draw));
            _apply.Add(new DmgDraw(draw));

            foreach (var apply in this._apply)
            {
                Console.WriteLine("Handling " + apply.Name);
                apply.HandleEvents();
            }
        }
    }
}
