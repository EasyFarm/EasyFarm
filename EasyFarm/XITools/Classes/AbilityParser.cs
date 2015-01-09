
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ZeroLimits.XITool.Enums;

namespace ZeroLimits.XITool.Classes
{
    /// <summary>
    /// A class for loading the ability and spell xmls from file.
    /// </summary>
    public class AbilityParser
    {
        public class RESOURCES
        {
            public const string ABILS_FILE_NAME = "abils.xml";
            public const string SPELLS_FILE_NAME = "spells.xml";
        }

        protected static XElement m_spellsDoc = null;
        protected static XElement m_abilsDoc = null;

        /// <summary>
        /// Class load time initializer
        /// </summary>
        static AbilityParser()
        {
            m_abilsDoc = LoadResource(RESOURCES.ABILS_FILE_NAME);
            m_spellsDoc = LoadResource(RESOURCES.SPELLS_FILE_NAME);
        }

        /// <summary>
        /// Ensures that the resource file passed exists
        /// and returns the XElement obj associated with the file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static XElement LoadResource(string filename)
        {
            XElement XMLDoc = null;

            String WorkingDirectory = Directory.GetCurrentDirectory();

            // Change to the resources directory if it exists.
            if (Directory.Exists("resources"))
            {
                Directory.SetCurrentDirectory("resources");

                // We can't operate without the resource files, shut it down.
                if (File.Exists(filename))
                {
                    XMLDoc = XElement.Load(filename);
                }

                // Revert to previous directory
                Directory.SetCurrentDirectory(WorkingDirectory);
            }

            return XMLDoc;
        }

        /// <summary>
        /// Grabs all abilities from the resource files with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected ICollection<Ability> ParseActions(String name)
        {
            // Parse spells and set spell status field.
            var spells = ParseResources("s", m_spellsDoc, name);
            foreach (var value in spells) value.ActionType = ActionType.Spell;

            // Parse abilities and set ability status field. 
            var abilities = ParseResources("a", m_abilsDoc, name);
            foreach (var value in abilities) value.ActionType = ActionType.Ability;

            // Create a list containing spells, abilities and items. 
            var resources = spells.Union(abilities).ToList();

            foreach (var value in resources)
            {
                if (string.Equals(value.Prefix, "/range", StringComparison.OrdinalIgnoreCase))
                    value.ActionType = ActionType.Ranged;
            }

            return resources;
        }

        /// <summary>
        /// Grabs all abilities from the resource files with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected ICollection<Ability> ParseAbilities(String name)
        {
            return ParseActions(name)
                .Where(x => x.ActionType == ActionType.Ability)
                .ToList();
        }

        /// <summary>
        /// Grabs all abilities from the resource files with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected ICollection<Ability> ParseSpells(String name)
        {
            return ParseActions(name)
                .Where(x => x.ActionType == ActionType.Spell)
                .ToList();
        }

        /// <summary>
        /// A general method for loading abilites from the .xml files. 
        /// </summary>
        /// <param name="pname">a, s or i for spell or ability</param>
        /// <param name="XDoc"></param>
        /// <param name="aname">Name of the ability to retrieve</param>
        /// <returns></returns>
        protected ICollection<Ability> ParseResources(String pname, XElement XDoc, String aname)
        {
            var Abilities = new List<Ability>();

            // Fetches the ability from xml.
            var element = XDoc.Elements(pname).Attributes()
                // enl check is for ability item names
                .Where(x => x.Name == "english")
                // Case insensitive match for the name. 
                .Where(x => x.Value.Equals(aname, StringComparison.CurrentCultureIgnoreCase))
                .Select(x => x.Parent);

            // Return blank if we did not find the ability.
            if (element == null) { return Abilities; }

            // Loop through all attributes and 
            // create an ability for each one. 
            foreach (var e in element)
            {
                Ability Ability = new Ability();

                Ability.Alias = (string)e.Attribute("alias");
                Ability.Element = (string)e.Attribute("element");
                Ability.Name = (string)e.Attribute("english");
                Ability.Prefix = (string)e.Attribute("prefix");
                Ability.Skill = (string)e.Attribute("skill");
                Ability.Targets = (string)e.Attribute("targets");
                Ability.Type = (string)e.Attribute("type");
                Ability.CastTime = (double?)e.Attribute("casttime") ?? 0;
                Ability.ID = (int?)e.Attribute("id") ?? 0;
                Ability.Index = (int?)e.Attribute("index") ?? 0;
                Ability.MPCost = (int?)e.Attribute("mpcost") ?? 0;
                Ability.Recast = (double?)e.Attribute("recast") ?? 0;
                Ability.TPCost = (int?)e.Attribute("tpcost") ?? 0;

                Abilities.Add(Ability);
            }

            return Abilities;
        }
    }
}