using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System.Collections.Generic;

namespace NechritoRiven
{
    internal class Program : Riven
    {
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            if (Player.ChampionName != "Riven")
                return;
            Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Nechrito Riven</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Version: 63 (Date: 25/4-16)</font></b>");
            Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Update</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\">Fast Laneclear + Jungle</font></b>");

          

             Console.WriteLine("Loading Menu...");
            MenuConfig.LoadMenu();
             Console.WriteLine("Loading Spells...");
            Spells.Initialise();
            
            Console.WriteLine("Loading Drawings...");
            DrawLoad.Load();

            Console.WriteLine("Loading Version...");
            Git.CheckVersion();

            Console.WriteLine("Nechrito Riven Loaded");
            /*
            Game.OnUpdate += Logic.Game_OnUpdate;
            Game.OnUpdate += OnTick;
           
            Obj_AI_Base.OnProcessSpellCast += Logic.OnCast;
            Obj_AI_Base.OnProcessSpellCast += OnCasting;
            Obj_AI_Base.OnDoCast += OnDoCast;
            Obj_AI_Base.OnDoCast += OnDoCastLc;
            Obj_AI_Base.OnPlayAnimation += OnPlay;
            Interrupter2.OnInterruptableTarget += Interrupt;
            */
        }
    }
}