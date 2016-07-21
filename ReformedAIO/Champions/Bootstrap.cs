using System;
using System.Collections.Generic;
using LeagueSharp;
using RethoughtLib.Classes.Bootstraps;
using RethoughtLib.Classes.Intefaces;

namespace ReformedAIO.Champions
{
    class Bootstrap : LeagueSharpAutoBootstrap
    {
        public Bootstrap(List<ILoadable> modules, List<string> additionalStrings = null) : base(modules, additionalStrings)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            Console.WriteLine("Reformed AIO - Loaded!");
            Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Reformed AIO</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Loaded</font></b>");
            Game.PrintChat("<b><font color=\"FFFFFF\">Don't forget to</font></b><b><font color=\"#00ff00\"> Upvote</font></b><b><font color=\"FFFFFF\"> In DB If You Liked It</font></b>");
        }
    }
}
