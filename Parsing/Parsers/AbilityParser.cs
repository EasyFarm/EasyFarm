
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

using Parsing.Abilities;
using Parsing.Augmenting;
using Parsing.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Parsing.Parsers
{
    /// <summary>
    /// A class for loading the ability and spell xmls from file.
    /// </summary>
    public class AbilityParser
    {
        /// <summary>
        /// Holds the list of loaded resource files. 
        /// </summary>
        public readonly IEnumerable<XElement> _resources;

        /// <summary>
        /// List of handlers to handle the conversion of abilities from file. 
        /// </summary>
        List<IObjectAugmenter<XElement, Ability>> _augmenters = 
            new List<IObjectAugmenter<XElement, Ability>>();

        /// <summary>
        /// Creates a parser that parsers resource files in the 
        /// given file path. 
        /// </summary>
        /// <param name="resourcesPath"></param>
        public AbilityParser(string resourcesPath)
        {
            // Read in all resources in the resourcePath. 
            _resources = LoadResources(resourcesPath);

            // Augmenters for predefined data types. 
            _augmenters.Add(new AbilityAugmenter<string>("alias", "Alias"));
            _augmenters.Add(new AbilityAugmenter<double>("casttime", "CastTime"));
            _augmenters.Add(new AbilityAugmenter<string>("element", "Element"));
            _augmenters.Add(new AbilityAugmenter<int>("id", "ID"));
            _augmenters.Add(new AbilityAugmenter<int>("index", "Index"));
            _augmenters.Add(new AbilityAugmenter<int>("mpcost", "MPCost"));
            _augmenters.Add(new AbilityAugmenter<string>("english", "English"));
            _augmenters.Add(new AbilityAugmenter<string>("japanese", "Japanese"));            
            _augmenters.Add(new AbilityAugmenter<string>("prefix", "Prefix"));
            _augmenters.Add(new AbilityAugmenter<double>("recast", "Recast"));
            _augmenters.Add(new AbilityAugmenter<string>("skill", "Skill"));
            _augmenters.Add(new AbilityAugmenter<string>("targets", "Targets"));
            _augmenters.Add(new AbilityAugmenter<int>("tpcost", "TPCost"));
            _augmenters.Add(new AbilityAugmenter<string>("type", "Type"));

            // Specialized augmenters. 
            _augmenters.Add(new AbilityTypeAugmenter("prefix", "AbilityType"));
            _augmenters.Add(new CategoryTypeAugmenter("type", "CategoryType"));
            _augmenters.Add(new ElementTypeAugmenter("element", "ElementType"));
            _augmenters.Add(new SkillTypeAugmenter("skill", "SkillType"));
            _augmenters.Add(new TargetTypeAugmenter("targets", "TargetType"));
        }

        /// <summary>
        /// Ensures that the resource file passed exists
        /// and returns the XElement obj associated with the file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerable<XElement> LoadResources(string path)
        {
            // List to store all read resources. 
            List<XElement> XmlDocuments = new List<XElement>();

            // Get a list of all resource file names. 
            string[] resources = Directory.GetFiles(path, "*.xml");

            // Load all resource files in the given directory. 
            foreach (var resource in resources)
            {
                XmlDocuments.Add(XElement.Load(resource));
            }

            return XmlDocuments;
        }

        /// <summary>
        /// Grabs all abilities from the resource files with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected IEnumerable<Ability> ParseAbilities(String name)
        {
            return ParseResources(name)
                .Where(x => CompositeAbilityTypes.IsAbility.HasFlag(x.AbilityType)
            );
        }

        /// <summary>
        /// Grabs all abilities from the resource files with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected IEnumerable<Ability> ParseSpells(String name)
        {
            return ParseResources(name)
                .Where(x => CompositeAbilityTypes.IsSpell.HasFlag(x.AbilityType)
            );
        }

        /// <summary>
        /// A general method for loading abilites from the .xml files. 
        /// </summary>
        /// <param name="pname">a, s or i for spell or ability</param>
        /// <param name="XDoc"></param>
        /// <param name="aname">Name of the ability to retrieve</param>
        /// <returns></returns>
        protected IEnumerable<Ability> ParseResources(string name)
        {
            // Stores the created abilities with the given name. 
            var abilities = new List<Ability>();

            // Stores the XElement attributes that match the name. 
            var elements = new List<XElement>();

            // Select all matching XElement objects. 
            foreach (var resource in _resources)
            {
                elements.AddRange(resource.Elements()
                .Attributes()
                .Where(x => x.Name != null && x.Name == "english" || x.Name == "japanese")
                .Where(x => x.Value.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                .Select(x => x.Parent));
            }

            // Start extracting values from XElement;s and augment our ability objects. 
            foreach (var element in elements.Where(x => x.HasAttributes))
            {
                Ability ability = new Ability();

                // Get the list of handlers that can process this element. 
                var augmenters = _augmenters
                    .Where(augmenter => augmenter.CanAugment(element))
                    .ToList();

                // Assign the ability's fields for each handler
                augmenters.ForEach(augmenter => augmenter.Augment(element, ability));

                abilities.Add(ability);
            }

            return abilities;
        }
    }
}