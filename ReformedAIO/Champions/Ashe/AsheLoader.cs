namespace ReformedAIO.Champions.Ashe
{
    #region Using Directives

    using System.Collections.Generic;
    using RethoughtLib.Utility;

    using LeagueSharp.Common;

    using Drawings;
    using Logic;
    using OrbwalkingMode.Combo;
    using OrbwalkingMode.JungleClear;
    using OrbwalkingMode.LaneClear;
    using OrbwalkingMode.Mixed;

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;

    #endregion

    internal class AsheLoader : LoadableBase
    {
        #region Public Properties

        public override string DisplayName { get; set; } = String.ToTitleCase("Reformed Ashe");

        public override string InternalName { get; set; } = "Ashe";

        public override IEnumerable<string> Tags { get; set; } = new[] {"Ashe"};

        #endregion

        #region Public Methods and Operators

        public override void Load()
        {
            var superParent = new SuperParent(DisplayName);

            var comboParent = new Parent("Combo");
            var mixedParent = new Parent("Mixed");
            var jungleParent = new Parent("JungleClear");
            var laneParent = new Parent("LaneClear");
            var drawingParent = new Parent("Drawings");

            superParent.AddChildren(new[]
            {
                comboParent, mixedParent, laneParent, jungleParent, drawingParent
            });

            var setSpell = new SetSpells();
            setSpell.Load();

            var qCombo = new QCombo("[Q]");
            var wCombo = new WCombo("[W]");
            var eCombo = new ECombo("[E]");
            var rCombo = new RCombo("[R]");

            comboParent.AddChild(qCombo);
            comboParent.AddChild(eCombo);
            comboParent.AddChild(wCombo);
            comboParent.AddChild(rCombo);

            var qMixed = new QMixed("[Q]");
            var wMixed = new WMixed("[W]");

            mixedParent.AddChild(qMixed);
            mixedParent.AddChild(wMixed);

            var qJungle = new QJungle("[Q]");
            var wJungle = new WJungle("[W]");

            jungleParent.AddChild(qJungle);
            jungleParent.AddChild(wJungle);

            var qLane = new QLane("[Q]");
            var wLane = new WLane("[W]");

            laneParent.AddChild(qLane);
            laneParent.AddChild(wLane);

            var wDraw = new WDraw("[W] Draw");
            var dmgDraw = new DmgDraw("Damage Indicator");

            drawingParent.AddChild(wDraw);
            drawingParent.AddChild(dmgDraw);

            var orbWalkingMenu = new Menu("Orbwalker", "Orbwalking");
            Variable.Orbwalker = new Orbwalking.Orbwalker(orbWalkingMenu);

            superParent.Menu.AddSubMenu(orbWalkingMenu);

            superParent.OnLoadInvoker();
        }

        #endregion
    }
}