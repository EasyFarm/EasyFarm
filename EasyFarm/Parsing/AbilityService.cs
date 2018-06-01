// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
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
using System.Collections;
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
        public List<Resource> Resources { get; set; }

        /// <summary>
        ///     Retrieve all resources within the given directory.
        /// </summary>
        /// <param name="resourcesPath"></param>
        public AbilityService(string resourcesPath)
        {
            // Read in all resources in the resourcePath.
            Resources = LoadResources(resourcesPath);
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
        ///     A general method for loading abilites from the .xml files.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<Resource> Find(Predicate<Resource> predicate)
        {
            return Resources.FindAll(predicate);
        }

        private static Resources.ResourceType ToResourceType(string name)
        {
                return (Resources.ResourceType) Enum.Parse(typeof(Resources.ResourceType),
                    name.Replace("_", ""), true);
        }

        /// <summary>
        ///     Ensures that the resource file passed exists
        ///     and returns the XElement obj associated with the file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<Resource> LoadResources(string path)
        {
            // List to store all read resources.
            // Get a list of all resource file names.
            if (!Directory.Exists(path)) return new List<Resource>();
            var files = Directory.GetFiles(path, "*.xml");

            // Load all resource files in the given directory.
            var resourceFiles = files.Select(XElement.Load).ToList();

            // Stores the created abilities with the given name.
            var resources = new List<Resource>();

            // Stores the XElement attributes that match the name.
            var elements = resourceFiles.SelectMany(x => x.Elements()).ToList();

            // Start extracting values from XElement;s and augment our ability objects.
            foreach (var element in elements.Where(x => x.HasAttributes))
            {
                var resource = new Resource
                {
                    ResourceType = ToResourceType(element.Parent.Name.LocalName),
                    Id = ToValue<string>(element, "id"),
                    En = ToValue<string>(element, "en"),
                    Ja = ToValue<string>(element, "ja"),
                    ActionId = ToValue<string>(element, "action_id"),
                    Color = ToValue<string>(element, "color"),
                    Suffix = ToValue<string>(element, "suffix"),
                    Prefix = ToValue<string>(element, "prefix"),
                    Access = ToValue<string>(element, "access"),
                    Command = ToValue<string>(element, "command"),
                    Equippable = ToValue<string>(element, "equippable"),
                    Enl = ToValue<string>(element, "enl"),
                    Jal = ToValue<string>(element, "jal"),
                    Incoming = ToValue<string>(element, "incoming"),
                    Outgoing = ToValue<string>(element, "outgoing"),
                    Element = ToValue<string>(element, "element"),
                    Alternative = ToValue<string>(element, "alternative"),
                    Flags = ToValue<string>(element, "flags"),
                    Stack = ToValue<string>(element, "stack"),
                    Type = ToValue<string>(element, "type"),
                    Targets = ToValue<string>(element, "targets"),
                    Category = ToValue<string>(element, "category"),
                    CastTime = ToValue<string>(element, "cast_time"),
                    Level = ToValue<string>(element, "level"),
                    Slots = ToValue<string>(element, "slots"),
                    Races = ToValue<string>(element, "races"),
                    Jobs = ToValue<string>(element, "jobs"),
                    MaxCharges = ToValue<string>(element, "max_charges"),
                    CastDelay = ToValue<string>(element, "cast_delay"),
                    RecastDelay = ToValue<string>(element, "recast_delay"),
                    ShieldSize = ToValue<string>(element, "shield_size"),
                    Damage = ToValue<string>(element, "damage"),
                    Delay = ToValue<string>(element, "delay"),
                    Skill = ToValue<string>(element, "skill"),
                    ItemLevel = ToValue<string>(element, "item_level"),
                    SuperiorLevel = ToValue<string>(element, "superior_level"),
                    Ens = ToValue<string>(element, "ens"),
                    Jas = ToValue<string>(element, "jas"),
                    IconId = ToValue<string>(element, "icon_id"),
                    MpCost = ToValue<string>(element, "mp_cost"),
                    RecastId = ToValue<string>(element, "recast_id"),
                    TpCost = ToValue<string>(element, "tp_cost"),
                    Duration = ToValue<string>(element, "duration"),
                    Range = ToValue<string>(element, "range"),
                    Endesc = ToValue<string>(element, "endesc"),
                    Jadesc = ToValue<string>(element, "jadesc"),
                    MonsterLevel = ToValue<string>(element, "monster_level"),
                    TpMoves = ToValue<string>(element, "tp_moves"),
                    Gender = ToValue<string>(element, "gender"),
                    Recast = ToValue<string>(element, "recast"),
                    Levels = ToValue<string>(element, "levels"),
                    IconIdNq = ToValue<string>(element, "icon_id_nq"),
                    Requirements = ToValue<string>(element, "requirements"),
                    Unlearnable = ToValue<string>(element, "unlearnable"),
                    SkillchainA = ToValue<string>(element, "skillchain_a"),
                    SkillchainB = ToValue<string>(element, "skillchain_c"),
                    SkillchainC = ToValue<string>(element, "skillchain_b"),
                    Intensity = ToValue<string>(element, "intensity"),
                    Search = ToValue<string>(element, "search"),
                };

                resources.Add(resource);
            }

            return resources.ToList();
        }

        public IEnumerable Find(Type type, Predicate<Resource> predicate = null)
        {
            predicate = predicate ?? (r => true);
            var resourceType = ToResourceType(type.Name);
            return Find(r => predicate(r) && r.ResourceType == resourceType).Select(r => r.As(type));
        }

        public IEnumerable<T> Find<T>(Predicate<Resource> predicate = null)
        {
            return Find(typeof(T), predicate).Cast<T>();
        }

        public IEnumerable<T> Find<T>(Predicate<T> predicate)
        {
            return Resources.Where(x => x.ResourceType == ToResourceType(typeof(T).Name))
                .Select(x => x.As<T>())
                .ToList()
                .FindAll(predicate);
        }

        public IEnumerable<Ability> GetAbilitiesWithName(string abilityName)
        {
            if (IsRangedCommand(abilityName))
            {
                return RangedCommand();
            }

            HashSet<Resources.ResourceType> resourceTypes = new HashSet<Resources.ResourceType>()
            {
                Parsing.Resources.ResourceType.Spells,
                Parsing.Resources.ResourceType.JobAbilities,
                Parsing.Resources.ResourceType.WeaponSkills,
                Parsing.Resources.ResourceType.Items,
            };

            IList<Resource> resources = Find(resource =>
                    resourceTypes.Contains(resource.ResourceType) &&
                    WithName(abilityName, resource))
                .ToList();

            IList<Ability> abilities = resources.Select(resource =>
            {
                var ability = new Ability
                {
                    Element = resource.Element,
                    English = resource.En,
                    Japanese = resource.Ja,
                    Prefix = resource.Prefix,
                    Skill = resource.Skill,
                    Targets = resource.Targets,
                    Type = resource.Type,
                    CastTime = ToValue<double>(resource.CastTime),
                    Recast = ToValue<double>(resource.Recast),
                    Id = ToValue<int>(resource.Id),
                    Index = ToValue<int>(resource.RecastId),
                    MpCost = ToValue<int>(resource.MpCost),
                    TpCost = ToValue<int>(resource.TpCost)
                };

                ability.Prefix = ResourceHelper.ToPrefix(resource);
                ability.AbilityType = ResourceHelper.ToAbilityType(ability.Prefix);
                ability.CategoryType = ResourceHelper.ToCategoryType(ability.Type);
                ability.ElementType = ResourceHelper.ToElementType(ability.Element);
                ability.SkillType = ResourceHelper.ToSkillType(ability.Skill);
                ability.TargetType = ResourceHelper.ToTargetTypeFlags(ability.Targets);

                if (ability.AbilityType == AbilityType.Weaponskill)
                {
                    ability.Index = 900;
                    ability.TpCost = 1000;
                }

                return ability;
            }).ToList();

            return abilities;
        }

        private static IEnumerable<Ability> RangedCommand()
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

        private static bool IsRangedCommand(string abilityName)
        {
            return string.Equals(abilityName, "Ranged", StringComparison.InvariantCultureIgnoreCase);
        }

        private Boolean WithName(String abilityName, Resource resource)
        {
            return String.Equals(resource.En, abilityName, StringComparison.InvariantCultureIgnoreCase) || 
                   String.Equals(resource.Ja, abilityName, StringComparison.InvariantCultureIgnoreCase);
        }

        private T ToValue<T>(string value)
        {
            if (value == null) return default(T);
            return (T) Convert.ChangeType(value, typeof(T));
        }
    }
}