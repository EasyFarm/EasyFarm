/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

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

using System.Collections.Generic;
using System.Linq;
using Parsing.Abilities;
using Parsing.Parsers;

namespace Parsing.Services
{
    /// <summary>
    ///     This class is responsible for retrieving job abilties and spells.
    /// </summary>
    public class AbilityService : AbilityParser, IAbilityService
    {
        public AbilityService(string resourcePath) :
            base(resourcePath)
        {
        }

        /// <summary>
        ///     Creates an ability obj. This object may be a spell
        ///     or an ability.
        /// </summary>
        /// <param name="name">Ability's Name</param>
        /// <returns>a new ability</returns>
        public Ability CreateAbility(string name)
        {
            var Abilities = GetAbilitiesWithName(name);
            if (Abilities.Count() <= 0) return new Ability();
            return Abilities.First();
        }

        /// <summary>
        ///     Returns a list of all abilities with a specific name.
        /// </summary>
        /// <param name="name">Name of the action</param>
        /// <returns>A list of actions with that name</returns>
        public IEnumerable<Ability> GetAbilitiesWithName(string name)
        {
            return ParseResources(name);
        }

        /// <summary>
        ///     Parses a resource in terms of an ability.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="XMLDoc"></param>
        /// <returns></returns>
        public IEnumerable<Ability> GetJobAbilitiesByName(string name)
        {
            return ParseAbilities(name);
        }

        /// <summary>
        ///     Parses a resource in terms of an spell.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="XMLDoc"></param>
        /// <returns></returns>
        public IEnumerable<Ability> GetSpellAbilitiesByName(string name)
        {
            return ParseSpells(name);
        }

        /// <summary>
        ///     Returns whether abilities with the given name.
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool Exists(string actionName)
        {
            return GetAbilitiesWithName(actionName).Count() > 0;
        }
    }
}