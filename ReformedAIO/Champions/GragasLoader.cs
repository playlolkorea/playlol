using System;
using System.Collections.Generic;
using LeagueSharp.Common;
using ReformedAIO.Champions.Gragas;
using ReformedAIO.Champions.Gragas.Logic;
using ReformedAIO.Champions.Gragas.Menus.Draw;
using ReformedAIO.Champions.Gragas.OrbwalkingMode.Combo;
using ReformedAIO.Champions.Gragas.OrbwalkingMode.Jungle;
using ReformedAIO.Champions.Gragas.OrbwalkingMode.Lane;
using ReformedAIO.Champions.Gragas.OrbwalkingMode.Mixed;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Classes.Intefaces;

namespace ReformedAIO.Champions
{
    class GragasLoader : ILoadable
    {
        private readonly List<IFeatureChild> _apply = new List<IFeatureChild>();

        public string Name { get; set; } = "Gragas";

        public void Load()
        {
            var rootMenu = new Menu("Gragas", "Gragas", true);
            rootMenu.AddToMainMenu();

            var orbWalkingMenu = new Menu("Orbwalker", "Orbwalking");
            Variable.Orbwalker = new Orbwalking.Orbwalker(orbWalkingMenu);
            rootMenu.AddSubMenu(orbWalkingMenu);

            var combo = new Combo(rootMenu);
            var mixed = new Mixed(rootMenu);
            var lane = new Lane(rootMenu);
            var jungle = new Jungle(rootMenu);
            var draw = new Draw(rootMenu);
            

            var qLogic = new QLogic();
            qLogic.Load();

            _apply.Add(new QCombo(combo));
            _apply.Add(new WCombo(combo));
            _apply.Add(new ECombo(combo));
            _apply.Add(new RCombo(combo));

            _apply.Add(new LaneQ(lane));
            _apply.Add(new LaneW(lane));
            _apply.Add(new LaneE(lane));

            _apply.Add(new QMixed(mixed));

            _apply.Add(new QJungle(jungle));
            _apply.Add(new WJungle(jungle));
            _apply.Add(new EJungle(jungle));
            
            _apply.Add(new DrawIndicator(draw));
            _apply.Add(new DrawQ(draw));
            _apply.Add(new DrawE(draw));
            _apply.Add(new DrawR(draw));
            
            SPrediction.Prediction.Initialize(rootMenu);

            foreach (var apply in this._apply)
            {
                Console.WriteLine("Handling " + apply.Name);
                apply.HandleEvents();
            }
        }
    }
}
