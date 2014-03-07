
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
    public class Abilities
    {
        private const string abils = "abils.xml";
        private const string spells = "spells.xml";
        private static XElement SpellsDoc = null;
        private static XElement AbilsDoc = null;

        /// <summary>
        /// Class load time initializer
        /// </summary>
        static Abilities()
        {
            AbilsDoc = LoadResource(abils);
            SpellsDoc = LoadResource(spells);
        }        

        /// <summary>
        /// Creates an ability obj. This object may be a spell
        /// or an ability.
        /// </summary>
        /// <param name="name">Ability's Name</param>
        /// <returns>a new ability</returns>
        public static Ability CreateAbility(string name)
        {
            // Get a job ability by name if one exists or return a blank one
            var JobAbility = GetJobAbilitiesByName(name).FirstOrDefault() ?? new Ability();
            var MagicAbility = GetSpellAbilitiesByName(name).FirstOrDefault() ?? new Ability();
            
            // If we've found a valid job ability, return that.
            if (JobAbility.IsValidName) { return JobAbility; }

            // If we've found a valid magic ability, return that.
            else if (MagicAbility.IsValidName) { return MagicAbility; }
            
            // Return a blank ability if we can't find the action.
            else { return new Ability(); }
        }

        /// <summary>
        /// Returns a list of all abilities with a specific name.
        /// </summary>
        /// <param name="name">Name of the action</param>
        /// <returns>A list of actions with that name</returns>
        public static List<Ability> GetAbilitiesWithName(String name)
        {
            var Abilities = new List<Ability>();
            Abilities.AddRange(GetSpellAbilitiesByName(name));
            Abilities.AddRange(GetJobAbilitiesByName(name));
            return Abilities;
        }

        /// <summary>
        /// Parses a resource in terms of an ability.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="XMLDoc"></param>
        /// <returns></returns>
        public static List<Ability> GetJobAbilitiesByName(string name)
        {
            List<Ability> Abilities = new List<Ability>();
            Abilities = ParseResources("a", AbilsDoc, name);
            Abilities.ForEach(x => x.IsAbility = true);
            return Abilities;
        }

        /// <summary>
        /// Parses a resource in terms of an spell.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="XMLDoc"></param>
        /// <returns></returns>
        /// 
        public static List<Ability> GetSpellAbilitiesByName(string name)
        {
            List<Ability> Abilities = new List<Ability>();
            Abilities = ParseResources("s", SpellsDoc, name);
            Abilities.ForEach(x => x.IsSpell = true);
            return Abilities;
        }

        private static List<Ability> ParseResources(String pname, XElement XDoc, String aname)
        {
            List<Ability> Abilities = new List<Ability>();

            // Fetches the ability from xml.
            var element = XDoc.Elements(pname).Attributes().Where(x => (x.Name == "english" && x.Value == aname)).Select(x => x.Parent);

            // Return blank if we did not find the ability.
            if (element == null) { return Abilities; }

            // Create a new ability from attributes in move.
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
                Ability.CastTime = (double)e.Attribute("casttime");
                Ability.ID = (int)e.Attribute("id");
                Ability.Index = (int)e.Attribute("index");
                Ability.MPCost = (int)e.Attribute("mpcost");
                Ability.Recast = (double)e.Attribute("recast");
                Ability.TPCost = (int?)e.Attribute("tpcost") ?? 0;

                Abilities.Add(Ability);
            }

            return Abilities;
        }

        /// <summary>
        /// Ensures that the resource file passed exists
        /// and returns the XElement obj associated with the file.
        /// </summary>
        /// <param name="abils"></param>
        /// <returns></returns>
        private static XElement LoadResource(string abils)
        {
            String WorkingDirectory = Directory.GetCurrentDirectory();

            // Change to the resources directory if it exists.
            if (Directory.Exists("resources"))
            {
                Directory.SetCurrentDirectory("resources");
            }

            // We can't operate without the resource files, shut it down.
            if (!File.Exists(abils))
            {                
                MessageBox.Show("Cannot find resources, shutting down application", 
                    "Resources Not Found: Exiting");
                System.Environment.Exit(0);
            }

            XElement XMLDoc = XElement.Load(abils);

            Directory.SetCurrentDirectory(WorkingDirectory);

            return XMLDoc;
        }      
    }
}
