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
    /// A class for loading the ability and spell xmls from file.
    /// </summary>
    public class AbilityParser
    {
        private const string ABILS = "abils.xml";
        private const string SPELLS = "spells.xml";
        protected static XElement _spelldoc = null;
        protected static XElement _abilsdoc = null;

        /// <summary>
        /// Class load time initializer
        /// </summary>
        static AbilityParser()
        {
            _abilsdoc = LoadResource(ABILS);
            _spelldoc = LoadResource(SPELLS);
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

        /// <summary>
        /// Grabs all abilities from the resource files with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected List<Ability> ParseActions(String name)
        {
            return ParseResources("s", _spelldoc, name)
                .Union(ParseResources("a", _abilsdoc, name))
                .ToList();
        }

        /// <summary>
        /// Grabs all abilities from the resource files with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected List<Ability> ParseAbilities(String name)
        {
            return ParseResources("a", _abilsdoc, name);
        }

        /// <summary>
        /// Grabs all abilities from the resource files with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected List<Ability> ParseSpells(String name)
        {
            return ParseResources("s", _spelldoc, name);
        }

        /// <summary>
        /// A general method for loading abilites from the .xml files. 
        /// </summary>
        /// <param name="pname">a or s for spell or ability</param>
        /// <param name="XDoc"></param>
        /// <param name="aname">Name of the ability to retrieve</param>
        /// <returns></returns>
        protected List<Ability> ParseResources(String pname, XElement XDoc, String aname)
        {
            var Abilities = new List<Ability>();

            // Fetches the ability from xml.
            var element = XDoc.Elements(pname).Attributes()
                .Where(x => (x.Name == "english" && x.Value == aname))
                .Select(x => x.Parent);

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
    }
}