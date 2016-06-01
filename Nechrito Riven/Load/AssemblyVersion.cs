using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using Version = System.Version;

namespace NechritoRiven.Load
{
    class AssemblyVersion
    {
        private static string DownloadServerVersion
        {
            get
            {
                using (var wC = new WebClient()) return wC.DownloadString("https://github.com/Nechrito/Leaguesharp/blob/master/Nechrito%20Riven/Properties/AssemblyInfo.cs");
            }
        }

        public static void CheckVersion()
        {
            try
            {
                var match = new Regex(@"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]").Match(DownloadServerVersion);

                if (!match.Success) return;
                var gitVersion = new Version($"{match.Groups[1]}.{match.Groups[2]}.{match.Groups[3]}.{match.Groups[4]}");

                if (gitVersion <= System.Reflection.Assembly.GetExecutingAssembly().GetName().Version) return;
                Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\"> Nechrito Riven</font></b><b><font color=\"#FFFFFF\">]</font></b> <font color=\"#FFFFFF\">Version:</font>{0} Available!", gitVersion);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\"> Nechrito Riven</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Could not get latest version</font></b>");
            }
        }
    }
}
