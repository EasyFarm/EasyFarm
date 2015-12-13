/*///////////////////////////////////////////////////////////////////
Copyright (C) <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using MemoryAPI;
using System.Collections.Generic;
using System.Linq;

namespace EasyFarm.Classes
{
    /// <summary>
    ///     Effects that render player actions unusable.
    /// </summary>
    public class ProhibitEffects
    {
        /// <summary>
        ///     Effects that prevent casting.
        /// </summary>
        public static IEnumerable<StatusEffect> ProhibitEffectsSpell
        {
            get
            {
                return new List<StatusEffect>
                {
                    StatusEffect.Silence,
                    StatusEffect.Mute
                }.Concat(ProhibitEffectsControl);
            }
        }

        /// <summary>
        ///     Effects that prevent ability usage.
        /// </summary>
        public static IEnumerable<StatusEffect> ProhibitEffectsAbility
        {
            get
            {
                return new List<StatusEffect>
                {
                    StatusEffect.Amnesia
                }.Concat(ProhibitEffectsControl);
            }
        }

        /// <summary>
        ///     Effects that prevent player action.
        /// </summary>
        public static IEnumerable<StatusEffect> ProhibitEffectsControl
        {
            get
            {
                return new List<StatusEffect>
                {
                    StatusEffect.Charm1,
                    StatusEffect.Charm2,
                    StatusEffect.Petrification,
                    StatusEffect.Sleep,
                    StatusEffect.Sleep2,
                    StatusEffect.Stun,
                    StatusEffect.Chocobo,
                    StatusEffect.Terror,
                    StatusEffect.Lullaby
                };
            }
        }

        /// <summary>
        ///     Effects that prevent player action.
        /// </summary>
        public static IEnumerable<StatusEffect> ProhibitEffectsMovement
        {
            get
            {
                return new List<StatusEffect>
                {
                    StatusEffect.Bind
                }.Concat(ProhibitEffectsControl);
            }
        }

        /// <summary>
        ///     Effects that prevent resting or recovering hp / mp.
        /// </summary>
        public static IEnumerable<StatusEffect> ProhibitEffectsDots
        {
            get
            {
                return new List<StatusEffect>
                {
                    StatusEffect.Bio,
                    StatusEffect.Poison,
                    StatusEffect.Frost,
                    StatusEffect.Burn,
                    StatusEffect.Choke,
                    StatusEffect.Rasp,
                    StatusEffect.Shock,
                    StatusEffect.Drown,
                    StatusEffect.Dia,
                    StatusEffect.Requiem,
                    StatusEffect.Disease,
                    StatusEffect.Helix,
                    StatusEffect.Plague
                };
            }
        }
    }
}