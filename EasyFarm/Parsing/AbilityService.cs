// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
//  
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using EliteMMO.API;
using MemoryAPI;

namespace EasyFarm.Parsing
{
    /// <summary>
    ///     A class for loading the ability and spell xmls from file.
    /// </summary>
    public class AbilityService
    {
        public List<Ability> Resources { get; set; } = new List<Ability>();

        /// <summary>
        ///     Retrieve all resources within the given directory.
        /// </summary>
        public AbilityService(IMemoryAPI api)
        {
            if (api == null) return;
            // Read in all resources in the resourcePath.
            Resources = LoadResources(api);
        }

        /// <summary>
        ///     Ensures that the resource file passed exists
        ///     and returns the XElement obj associated with the file.
        /// </summary>
        /// <returns></returns>
        private List<Ability> LoadResources(IMemoryAPI api)
        {
            List<Ability> resources = new List<Ability>();

            SpellMapper spellMapper = new SpellMapper();
            AbilityMapper abilityMapper = new AbilityMapper();
            ItemMapper itemMapper = new ItemMapper();

            for (Int32 i = 0; i < 100000; i++)
            {
                EliteAPI.IItem item = api.Resource.GetItem(i);
                EliteAPI.ISpell spell = api.Resource.GetSpell(i);
                EliteAPI.IAbility ability = api.Resource.GetAbility(i);

                if (item != null)
                    resources.Add(itemMapper.Map(item));
                if (spell != null)
                    resources.Add(spellMapper.Map(spell));
                if (ability != null)
                    resources.Add(abilityMapper.Map(ability));
            }

            return resources;
        }

        public IEnumerable<Ability> GetAbilitiesWithName(String abilityName)
        {
            if (IsRangedCommand(abilityName))
                return RangedCommand();
            else
                return Resources.Where(x => x.English.Equals(abilityName, StringComparison.CurrentCultureIgnoreCase))
                    .ToList();
        }

        private IEnumerable<Ability> RangedCommand()
        {
            return new[]
            {
                new Ability()
                {
                    AbilityType = AbilityType.Range,
                    TargetType = TargetType.Enemy,
                    Prefix = "/range",
                    English = "Ranged"
                }
            };
        }

        private Boolean IsRangedCommand(String abilityName)
        {
            return String.Equals(abilityName, "Ranged", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}