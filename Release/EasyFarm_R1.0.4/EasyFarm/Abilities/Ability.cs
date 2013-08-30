using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FFACETools;

namespace EasyFarm.PlayerTools
{
    /// <summary>
    /// An action to be used on a target unit or player.
    /// Could be a spell or an ability.
    /// </summary>
    public class Ability
    {
        public int ID, Index, MPCost, TPCost, Recast;
        public double CastTime;
        public string Prefix, Name, Type, Element, Targets, Skill, Alias;
        public string Postfix = "";
        public bool IsValidName { get { return !string.IsNullOrWhiteSpace(Name); } }
        public bool IsSpell, IsAbility;

        /// <summary>
        /// Creates a blank ability.
        /// </summary>
        public Ability()
        {

        }

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
        /// or an ability. It first tries to create an ability and if
        /// that fails, creates an spell obj.
        /// </summary>
        /// <param name="name">Ability's Name</param>
        /// <returns>a new ability</returns>
        public static Ability CreateAbility(string name)
        {
            const string abils = "abils.xml";
            const string spells = "spells.xml";

            // try to get an ability
            Ability Result = ParseAbility(name, abils);

            // if we failed, try to get a spell.
            if (!Result.IsValidName)
                Result = ParseSpell(name, spells);

            return Result;
        }

        /// <summary>
        /// Tries to obtain valid ability obj.
        /// If it fails, it will return null.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="abils"></param>
        /// <returns></returns>
        private static Ability ParseAbility(string name, string abils)
        {
            Ability Result = new Ability();

            XElement XMLDoc = LoadResource(abils);
            
            if (!XMLDoc.HasElements)
                return Result;

            var Query = ParseAbilityXML(name, XMLDoc);

            // Did we fail to get an ability obj?
            if (Query.Count() <= 0)
            {
                Result = new Ability();
            }
            // We got a valid ability obj, 
            // save it and set is ability to true.
            else
            {
                Result = Query.Single();
                Result.IsAbility = true;
            }

            return Result;
        }

        /// <summary>
        /// Parses a resource in terms of an ability.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="XMLDoc"></param>
        /// <returns></returns>
        private static IEnumerable<Ability> ParseAbilityXML(string name, XElement XMLDoc)
        {
            var Query = from i in XMLDoc.Elements("a")
                        let Name = (string)i.Attribute("english")
                        where Name == name
                        let ID = (int)i.Attribute("id")
                        let Index = (int)i.Attribute("index")
                        let Prefix = (string)i.Attribute("prefix")
                        let Type = (string)i.Attribute("type")
                        let Element = (string)i.Attribute("element")
                        let Targets = (string)i.Attribute("targets")
                        let Skill = (string)i.Attribute("skill")
                        let MPCost = (int)i.Attribute("mpcost")
                        let TPCost = (int)i.Attribute("tpcost")
                        let CastTime = (double)i.Attribute("casttime")
                        let RecastTime = (int)i.Attribute("recast")
                        let Alias = (string)i.Attribute("alias")

                        select new Ability
                        {
                            Alias = Alias,
                            CastTime = CastTime,
                            Element = Element,
                            ID = ID,
                            Index = Index,
                            MPCost = MPCost,
                            Name = Name,
                            Prefix = Prefix,
                            Recast = RecastTime,
                            Skill = Skill,
                            Targets = Targets,
                            TPCost = TPCost,
                            Type = Type
                        };
            return Query;
        }

        /// <summary>
        /// Tries to obtain a valid spell obj.
        /// if it fails, will return null.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="abils"></param>
        /// <returns></returns>
        private static Ability ParseSpell(string name, string abils)
        {
            Ability Result = new Ability();

            XElement XMLDoc = LoadResource(abils);

            if (!XMLDoc.HasElements)
                return Result;
            
            var Query = ParseSpellXML(name, XMLDoc);

            // Did we fail to get an spell?
            if (Query.Count() <= 0)
            {
                Result = new Ability();
            }
            // We succeeded, get the spell and 
            // set is spell to true.
            else
            {
                Result = Query.Single();
                Result.IsSpell = true;
            }

            return Result;
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
        /// Parses a resource in terms of an spell.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="XMLDoc"></param>
        /// <returns></returns>
        private static IEnumerable<Ability> ParseSpellXML(string name, XElement XMLDoc)
        {
            var Query = from i in XMLDoc.Elements("s")
                        let Name = (string)i.Attribute("english")
                        where Name == name
                        let ID = (int)i.Attribute("id")
                        let Index = (int)i.Attribute("index")
                        let Prefix = (string)i.Attribute("prefix")
                        let Type = (string)i.Attribute("type")
                        let Element = (string)i.Attribute("element")
                        let Targets = (string)i.Attribute("targets")
                        let Skill = (string)i.Attribute("skill")
                        let MPCost = (int)i.Attribute("mpcost")
                        let CastTime = (double)i.Attribute("casttime")
                        let RecastTime = (int)i.Attribute("recast")
                        let Alias = (string)i.Attribute("alias")

                        select new Ability
                        {
                            Alias = Alias,
                            CastTime = CastTime,
                            Element = Element,
                            ID = ID,
                            Index = Index,
                            MPCost = MPCost,
                            Name = Name,
                            Prefix = Prefix,
                            Recast = RecastTime,
                            Skill = Skill,
                            Targets = Targets,
                            Type = Type
                        };
            return Query;
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
