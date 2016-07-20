using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace ReformedAIO.Champions.Ashe.Logic
{
    class ELogic
    {

        public bool CanCastE()
        {
            var pos = Camp.FirstOrDefault(x => x.Value.Distance(Variable.Player.Position) > 1500 && x.Value.Distance(Variable.Player.Position) < 7000);

            return pos.Value.IsValid();
        }

        public readonly Dictionary<string, Vector3> Camp = new Dictionary<string, Vector3>()
        {
            { "mid_Dragon" , new Vector3 (9122f, 4058f, 53.95995f) },
            { "left_dragon" , new Vector3 (9088f, 4544f, 52.24316f) },
            { "baron" , new Vector3 (5774f, 10706f, 55.77578F) }, //not pre 20
            { "red_wolves" , new Vector3 (11772f, 8856f, 50.30728f) },
            { "blue_wolves" , new Vector3 (3046f, 6132f, 57.04655f) },
        };

        public bool ComboE(Obj_AI_Hero target)
        {
            return !target.IsVisible && !target.IsDead && target.Distance(Variable.Player) < 1500f;
        }

        public int GetEAmmo()
        {
            return Variable.Player.Spellbook.GetSpell(SpellSlot.E).Ammo;
        }
    }
}
