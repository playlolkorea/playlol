
using LeagueSharp.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nechrito_Twitch
{
    class Usables
    {
        public static void CastYoumoo()
        {
            if (ItemData.Youmuus_Ghostblade.GetItem().IsReady()) ItemData.Youmuus_Ghostblade.GetItem().Cast();
        }

        public static void Botrk()
        {
            if (ItemData.Blade_of_the_Ruined_King.GetItem().IsReady()) ItemData.Blade_of_the_Ruined_King.GetItem().Cast();
        }
    }
}
