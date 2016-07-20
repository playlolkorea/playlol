using System;
using System.Collections.Generic;
using LeagueSharp.Common;
using ReformedAIO.Champions.Diana;
using ReformedAIO.Champions.Diana.Logic.Killsteal;
using ReformedAIO.Champions.Diana.Menus.Draw;
using ReformedAIO.Champions.Diana.OrbwalkingMode.Combo;
using ReformedAIO.Champions.Diana.OrbwalkingMode.Flee;
using ReformedAIO.Champions.Diana.OrbwalkingMode.Jungleclear;
using ReformedAIO.Champions.Diana.OrbwalkingMode.Laneclear;
using ReformedAIO.Champions.Diana.OrbwalkingMode.Misaya;
using ReformedAIO.Champions.Diana.OrbwalkingMode.Mixed;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Classes.Intefaces;
using CrescentStrike = ReformedAIO.Champions.Diana.OrbwalkingMode.Combo.CrescentStrike;
using LunarRush = ReformedAIO.Champions.Diana.OrbwalkingMode.Combo.LunarRush;
using Moonfall = ReformedAIO.Champions.Diana.OrbwalkingMode.Combo.Moonfall;
using PaleCascade = ReformedAIO.Champions.Diana.OrbwalkingMode.Combo.PaleCascade;

namespace ReformedAIO.Champions
{
    class DianaLoader : ILoadable
    {
        private readonly List<IFeatureChild> _apply = new List<IFeatureChild>();

        public string Name { get; set; } = "Diana";

        public void Load()
        {
            var rootMenu = new Menu("Diana", "Diana", true);
            rootMenu.AddToMainMenu();

            var orbWalkingMenu = new Menu("Orbwalker", "Orbwalking");
            Variables.Orbwalker = new Orbwalking.Orbwalker(orbWalkingMenu);
            rootMenu.AddSubMenu(orbWalkingMenu);

            // Parents
            var combo = new Combo(rootMenu);
            var misaya = new Misaya(rootMenu);
            var mixed = new Mixed(rootMenu);
            var lane = new Laneclear(rootMenu);
            var jungle = new Jungleclear(rootMenu);
            var ks = new Killsteal(rootMenu);
            var draw = new Draw(rootMenu);
            var flee = new Flee(rootMenu);

            _apply.Add(new CrescentStrike(combo));
            _apply.Add(new Moonfall(combo));
            _apply.Add(new LunarRush(combo));
            _apply.Add(new PaleCascade(combo));

            _apply.Add(new MisayaCombo(misaya));

            _apply.Add(new MixedCrescentStrike(mixed));

            _apply.Add(new LaneCrescentStrike(lane));
            _apply.Add(new LaneLunarRush(lane));

            _apply.Add(new JungleCrescentStrike(jungle));
            _apply.Add(new JungleLunarRush(jungle));
            _apply.Add(new JungleMoonfall(jungle));
            _apply.Add(new JunglePaleCascade(jungle));

            _apply.Add(new ksPaleCascade(ks));
            _apply.Add(new ksCrescentStrike(ks));

            _apply.Add(new DrawQ(draw));
            _apply.Add(new DrawE(draw));
            _apply.Add(new DrawDmg(draw));
            _apply.Add(new DrawPred(draw));

            _apply.Add(new FleeMode(flee));

            foreach (var apply in this._apply)
            {
                Console.WriteLine("Handling " + apply.Name);
                apply.HandleEvents();
            }
        }
    }
}
