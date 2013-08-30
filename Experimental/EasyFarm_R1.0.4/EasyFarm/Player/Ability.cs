using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FFACETools;
using System.Diagnostics;
using System.Xml;

namespace EasyFarm.PlayerTools
{
    /// <summary>
    /// An action to be used on a target unit or player.
    /// Could be a spell or an ability.
    /// </summary>
    public class Ability
    {
        public int ID, Index, MPCost, TPCost;
        public double CastTime, Recast;
        public string Prefix, Name, Type, Element, Targets, Skill, Alias;
        public string Postfix = "";
        public bool IsValidName { get { return !string.IsNullOrWhiteSpace(Name); } }
        public bool IsSpell, IsAbility;

        /// <summary>
        /// Creates a blank ability.
        /// </summary>
        public Ability() { }

        /// <summary>
        /// Creates an ability by name and sets it's 
        /// IsAbility and IsSpell Fields.
        /// </summary>
        /// <param name="name"></param>
        public Ability(string name)
        {
            Ability Ability = CreateAbility(name);

            this.ID = Ability.ID;
            this.Index = Ability.Index;
            this.MPCost = Ability.MPCost;
            this.TPCost = Ability.TPCost;
            this.CastTime = Ability.CastTime;
            this.Recast = Ability.Recast;
            this.Prefix = Ability.Prefix;
            this.Name = Ability.Name;
            this.Type = Ability.Type;
            this.Element = Ability.Element;
            this.Targets = Ability.Targets;
            this.Skill = Ability.Skill;
            this.Alias = Ability.Alias;
            this.Postfix = Ability.Postfix;
            this.IsSpell = Ability.IsSpell;
            this.IsAbility = Ability.IsAbility;
        }

        /// <summary>
        /// Creates an ability obj. This object may be a spell
        /// or an ability.
        /// </summary>
        /// <param name="name">Ability's Name</param>
        /// <returns>a new ability</returns>
        public static Ability CreateAbility(string name)
        {
            const string abils = "abils.xml";
            const string spells = "spells.xml";
            XElement XMLDoc = LoadResource(abils);
            var JobAbility = ParseAbilityXML(name, XMLDoc);
            XMLDoc = LoadResource(spells);
            var MagicAbility = ParseSpellXML(name, XMLDoc);
            return JobAbility.IsValidName ? JobAbility : MagicAbility;
        }

        /// <summary>
        /// Parses a resource in terms of an ability.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="XMLDoc"></param>
        /// <returns></returns>
        private static Ability ParseAbilityXML(string name, XElement XMLDoc)
        {
            // Fetches the ability from xml.
            XElement element = XMLDoc.Elements("a").Attributes().Where(x => x.Name == "english" && x.Value == name).Select(x => x.Parent).SingleOrDefault();

            // Return blank if we did not find the ability.
            if (element == null)
                return new Ability();

            // Create a new ability from attributes in move.
            return new Ability()
            {
                Alias = (string)element.Attribute("alias"),                
                Element = (string)element.Attribute("element"),
                Name = (string)element.Attribute("english"),
                Prefix = (string)element.Attribute("prefix"),
                Skill = (string)element.Attribute("skill"),
                Targets = (string)element.Attribute("targets"),
                Type = (string)element.Attribute("type"),
                CastTime = (double)element.Attribute("casttime"),
                ID = (int)element.Attribute("id"),
                Index = (int)element.Attribute("index"),
                MPCost = (int)element.Attribute("mpcost"),
                Recast = (double)element.Attribute("recast"),
                TPCost = (int)element.Attribute("tpcost"),
                IsAbility = true
            };
        }

        /// <summary>
        /// Parses a resource in terms of an spell.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="XMLDoc"></param>
        /// <returns></returns>
        /// 

        /* Sleepga II matches twice, fix*/
        private static Ability ParseSpellXML(string name, XElement XMLDoc)
        {
            // Fetches the ability from xml.
            XElement element = XMLDoc.Elements("s").Attributes().Where(x => (x.Name == "english" && x.Value == name)).Select(x => x.Parent).SingleOrDefault();

            // Return blank if we did not find the ability.
            if (element == null)
                return new Ability();

            // Create a new ability from attributes in move.
            return new Ability()
            {
                Alias = (string)element.Attribute("alias"),
                Element = (string)element.Attribute("element"),
                Name = (string)element.Attribute("english"),
                Prefix = (string)element.Attribute("prefix"),
                Skill = (string)element.Attribute("skill"),
                Targets = (string)element.Attribute("targets"),
                Type = (string)element.Attribute("type"),
                CastTime = (double)element.Attribute("casttime"),
                ID = (int)element.Attribute("id"),
                Index = (int)element.Attribute("index"),
                MPCost = (int)element.Attribute("mpcost"),
                Recast = (double)element.Attribute("recast"),
                IsSpell = true
            };
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

            if (Directory.Exists("resources"))
                Directory.SetCurrentDirectory("resources");

            if (!File.Exists(abils))
                return null;

            XElement XMLDoc = XElement.Load(abils);

            Directory.SetCurrentDirectory(WorkingDirectory);

            return XMLDoc;
        }

        /// <summary>
        /// Returns the command to execute the ability or spell
        ///      /ma "Dia" <t>
        ///      /ja "Provoke" <t>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            switch (Targets)
            {
                case "Enemy":
                    Postfix = "<t>";
                    break;
                case "Self":
                    Postfix = "<me>";
                    break;
                case "Self, Party":
                    Postfix = "<me>";
                    break;
                case "Self, Player, Party, Ally, NPC, Enemy":
                    Postfix = "<me>";
                    break;
                default:
                    break;
            }

            if (Prefix == "/range")
                return Prefix + " " + Postfix;
            else
                return Prefix + " \"" + Name + "\" " + Postfix;
        }
    }
}
