using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System;
using System.Linq;

namespace NechritoRiven
{
    class DrawLoad : Riven
    {
        public static void Load()
        {
            Drawing.OnDraw += Drawings.Drawing_OnDraw;
            Drawing.OnEndScene += Drawings.Drawing_OnEndScene;
        }
    }
}
