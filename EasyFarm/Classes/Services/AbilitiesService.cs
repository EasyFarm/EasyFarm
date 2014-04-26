
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013 - 2014>  <Zerolimits>

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace EasyFarm.Classes
{
    /// <summary>
    /// This class is responsible for retrieving job abilties and spells.
    /// </summary>
    public class AbilityService : AbilityParser
    {
        /// <summary>
        /// Creates an ability obj. This object may be a spell
        /// or an ability.
        /// </summary>
        /// <param name="name">Ability's Name</param>
        /// <returns>a new ability</returns>
        public Ability CreateAbility(string name)
        {
            var Abilities = GetAbilitiesWithName(name);

            if (Abilities.Count <= 0)
                return new Ability();
            else
                return Abilities.First();
        }

        /// <summary>
        /// Returns a list of all abilities with a specific name.
        /// </summary>
        /// <param name="name">Name of the action</param>
        /// <returns>A list of actions with that name</returns>
        public List<Ability> GetAbilitiesWithName(String name)
        {
            return GetJobAbilitiesByName(name)
                .Union(GetSpellAbilitiesByName(name))
                .ToList();
        }

        /// <summary>
        /// Parses a resource in terms of an ability.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="XMLDoc"></param>
        /// <returns></returns>
        public List<Ability> GetJobAbilitiesByName(string name)
        {
            return ParseAbilities(name).FindAll(x => x.IsAbility = true);
        }

        /// <summary>
        /// Parses a resource in terms of an spell.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="XMLDoc"></param>
        /// <returns></returns>
        /// 
        public List<Ability> GetSpellAbilitiesByName(string name)
        {
            return ParseSpells(name).FindAll(x => x.IsSpell = true);
        }

        /// <summary>
        /// Returns whether abilities with the given name.
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool Exists(string actionName)
        {
            return GetAbilitiesWithName(actionName).Count > 0;
        }
    }
}
