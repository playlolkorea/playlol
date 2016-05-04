using System.Collections.Generic;
using LeagueSharp.Common;
using SharpDX;

namespace NechritoRiven
{
    public struct Creatures
    {
        public static Dictionary<string, Vector2> MonsterLocations = new Dictionary<string, Vector2>()
        {
            {"Neutral.Dragon",SummonersRift.River.Dragon},
            {"Neutral.Baron",SummonersRift.River.Baron},

            {"Chaos.Red",new Vector2(7016.869f, 10775.55f)},
            {"Chaos.Blue",new Vector2(10931.73f, 6990.844f)},

            {"Order.Red",new Vector2(7862.244f, 4111.187f)},
            {"Order.Blue",new Vector2(3871.489f, 7901.054f)}
        };
    }
}
