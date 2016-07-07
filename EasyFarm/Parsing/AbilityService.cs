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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace EasyFarm.Parsing
{
    /// <summary>
    ///     A class for loading the ability and spell xmls from file.
    /// </summary>
    public class AbilityService
    {
        /// <summary>
        ///     A collection of resources to search for values.
        /// </summary>
        protected readonly IEnumerable<XElement> Resources;

        /// <summary>
        ///     Retrieve all resources within the given directory.
        /// </summary>
        /// <param name="resourcesPath"></param>
        public AbilityService(string resourcesPath)
        {
            // Read in all resources in the resourcePath.
            Resources = LoadResources(resourcesPath);
        }

        /// <summary>
        ///     Creates an ability obj. This object may be a spell
        ///     or an ability.
        /// </summary>
        /// <param name="name">Ability's Name</param>
        /// <returns>a new ability</returns>
        public Ability CreateAbility(string name)
        {
            var abilities = GetAbilitiesWithName(name);
            var enumerable = abilities as Ability[] ?? abilities.ToArray();
            if (!enumerable.Any()) return new Ability();
            return enumerable.First();
        }

        /// <summary>
        ///     Returns whether abilities with the given name.
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool Exists(string actionName)
        {
            return GetAbilitiesWithName(actionName).Any();
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
        /// <returns></returns>
        public IEnumerable<Ability> GetJobAbilitiesByName(string name)
        {
            return ParseAbilities(name);
        }

        /// <summary>
        ///     Parses a resource in terms of an spell.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<Ability> GetSpellAbilitiesByName(string name)
        {
            return ParseSpells(name);
        }

        public T ToValue<T>(XElement element, string attributeName)
        {
            if (element.HasAttributes && element.Attribute(attributeName) != null)
            {
                var value = element.Attribute(attributeName).Value;
                return (T)Convert.ChangeType(value, typeof (T), CultureInfo.InvariantCulture);
            }

            return default(T);
        }

        /// <summary>
        ///     Grabs all abilities from the resource files with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected IEnumerable<Ability> ParseAbilities(string name)
        {
            return ParseResources(name).Where(x => ResourceHelper.IsAbility(x.AbilityType));
        }

        /// <summary>
        ///     A general method for loading abilites from the .xml files.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected IEnumerable<Ability> ParseResources(string name)
        {
            // Stores the created abilities with the given name.
            var abilities = new List<Ability>();

            // Stores the XElement attributes that match the name.
            var elements = new List<XElement>();

            // Select all matching XElement objects.
            foreach (var resource in Resources)
            {
                elements.AddRange(resource.Elements()
                    .Attributes()
                    .Where(x => x.Name == "english" || x.Name == "japanese")
                    .Where(x => x.Value.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    .Select(x => x.Parent));
            }

            // Start extracting values from XElement;s and augment our ability objects.
            foreach (var element in elements.Where(x => x.HasAttributes))
            {
                var ability = new Ability();

                ability.Alias = ToValue<string>(element, "alias");
                ability.Element = ToValue<string>(element, "element");
                ability.English = ToValue<string>(element, "english");
                ability.Japanese = ToValue<string>(element, "japanese");
                ability.Prefix = ToValue<string>(element, "prefix");
                ability.Skill = ToValue<string>(element, "skill");
                ability.Targets = ToValue<string>(element, "targets");
                ability.Type = ToValue<string>(element, "type");

                ability.CastTime = ToValue<double>(element, "casttime");
                ability.Recast = ToValue<double>(element, "recast");

                ability.Id = ToValue<int>(element, "id");
                ability.Index = ToValue<int>(element, "index");
                ability.MpCost = ToValue<int>(element, "mpcost");
                ability.TpCost = ToValue<int>(element, "tpcost");

                ability.AbilityType = ResourceHelper.ToAbilityType(ability.Prefix);
                ability.CategoryType = ResourceHelper.ToCategoryType(ability.Type);
                ability.ElementType = ResourceHelper.ToElementType(ability.Element);
                ability.SkillType = ResourceHelper.ToSkillType(ability.Skill);
                ability.TargetType = ResourceHelper.ToTargetTypeFlags(ability.Targets);

                abilities.Add(ability);
            }

            return abilities;
        }

        /// <summary>
        ///     Grabs all abilities from the resource files with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected IEnumerable<Ability> ParseSpells(string name)
        {
            return ParseResources(name).Where(x => ResourceHelper.IsSpell(x.AbilityType));
        }

        /// <summary>
        ///     Ensures that the resource file passed exists
        ///     and returns the XElement obj associated with the file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerable<XElement> LoadResources(string path)
        {
            // List to store all read resources.
            // Get a list of all resource file names.
            if (!Directory.Exists(path)) return new List<XElement>();
            var resources = Directory.GetFiles(path, "*.xml");

            // Load all resource files in the given directory.
            return resources.Select(XElement.Load).ToList();
        }
    }
}