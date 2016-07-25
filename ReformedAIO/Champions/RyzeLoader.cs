using ReformedAIO.Champions.Ryze;
using ReformedAIO.Champions.Ryze.Drawings;
using ReformedAIO.Champions.Ryze.Killsteal;
using ReformedAIO.Champions.Ryze.Logic;
using ReformedAIO.Champions.Ryze.OrbwalkingMode.Combo;
using ReformedAIO.Champions.Ryze.OrbwalkingMode.Jungle;
using ReformedAIO.Champions.Ryze.OrbwalkingMode.Lane;
using ReformedAIO.Champions.Ryze.OrbwalkingMode.Mixed;

using System;
using System.Collections.Generic;
using LeagueSharp.Common;

using RethoughtLib.Classes.Feature;
using RethoughtLib.Classes.Intefaces;

namespace ReformedAIO.Champions
{
    internal class RyzeLoader : ILoadable
    {
        private readonly List<IFeatureChild> _apply = new List<IFeatureChild>();

        public string Name { get; set; } = "Ryze";

        public void Load()
        {
            var rootMenu = new Menu("Ryze", "Ryze", true);
            rootMenu.AddToMainMenu();

            var orbWalkingMenu = new Menu("Orbwalker", "Orbwalking");
            Variable.Orbwalker = new Orbwalking.Orbwalker(orbWalkingMenu);
            rootMenu.AddSubMenu(orbWalkingMenu);

            var setSpells = new SetSpells();
            setSpells.Load();

            var comboParent = new Combo(rootMenu);
            var laneParent = new Lane(rootMenu);
            var jungleParent = new Jungle(rootMenu);
            var mixedParent = new Mixed(rootMenu);
            var killsteal = new Killsteal(rootMenu);
            var drawParent = new Draw(rootMenu);

            _apply.Add(new RyzeCombo(comboParent));
            
            _apply.Add(new QMixed(mixedParent));
            _apply.Add(new WMixed(mixedParent));
            _apply.Add(new EMixed(mixedParent));

            _apply.Add(new QLane(laneParent));
            _apply.Add(new WLane(laneParent));
            _apply.Add(new ELane(laneParent));

            _apply.Add(new QJungle(jungleParent));
            _apply.Add(new WJungle(jungleParent));
            _apply.Add(new EJungle(jungleParent));

            _apply.Add(new KillstealMenu(killsteal));

            _apply.Add(new QDraw(drawParent));
            _apply.Add(new EDraw(drawParent));
            _apply.Add(new RDraw(drawParent));
            _apply.Add(new DmgDraw(drawParent));

            foreach (var apply in this._apply)
            {
                Console.WriteLine("Handling " + apply.Name);
                apply.HandleEvents();
            }
        }
    }
}
