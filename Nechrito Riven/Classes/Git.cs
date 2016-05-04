using LeagueSharp;
using System;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NechritoRiven
{
    class Git
    {
        public static void CheckVersion()
        {
            try
            {
                var match = new Regex(@"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]").Match(DownloadServerVersion());

                if (!match.Success) return;
                var gitVersion = new Version(string.Format("{0}.{1}.{2}.{3}",match.Groups[1],match.Groups[2],match.Groups[3],match.Groups[4]));

                if (gitVersion <= Assembly.GetExecutingAssembly().GetName().Version) return;
                Game.PrintChat("<b> <font color=\"#F88017\">S+</font> Class <font color=\"#F88017\">Riven</font></b> - <font color=\"#008080\">Version:</font>{0} Available!", gitVersion);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Game.PrintChat("<b> <font color=\"#008080\">Nechrito Riven Cannot Load Versions!</font></b>");
            }
        }
        private static string DownloadServerVersion()
        {
            using (var wC = new WebClient()) return wC.DownloadString("https://github.com/Nechrito/Leaguesharp/blob/master/Nechrito%20Riven/Properties/AssemblyInfo.cs");
        }
    }
}
