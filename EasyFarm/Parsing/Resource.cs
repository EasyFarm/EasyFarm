using System;

namespace EasyFarm.Parsing
{
    /// <summary>
    /// Auto-generated from the Windower resource files.
    /// </summary>
    public class Resources
    {
        public enum ResourceType
        {
            Unknown,
            AbilityRecasts,
            ActionMessages,
            Augments,
            AutoTranslates,
            Bags,
            Buffs,
            Chat,
            CheckRatings,
            Days,
            Elements,
            Emotes,
            Encumbrance,
            Items,
            ItemDescriptions,
            Jobs,
            JobAbilities,
            JobPoints,
            JobTraits,
            KeyItems,
            MeritPoints,
            MonsterAbilities,
            Monstrosity,
            MoonPhases,
            Mounts,
            Races,
            Regions,
            Servers,
            Skills,
            Slots,
            Spells,
            Statuses,
            SynthRanks,
            Titles,
            WeaponSkills,
            Weather,
            Zones,
        }

        public class AbilityRecasts
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
            public int ActionId { get; set; }
        }

        public class ActionMessages
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Color { get; set; }
            public string Suffix { get; set; }
            public string Prefix { get; set; }
        }

        public class Augments
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
        }

        public class AutoTranslates
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
        }

        public class Bags
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Access { get; set; }
            public string Command { get; set; }
            public bool Equippable { get; set; }
        }

        public class Buffs
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Enl { get; set; }
            public string Ja { get; set; }
            public string Jal { get; set; }
        }

        public class Chat
        {
            public int Id { get; set; }
            public string En { get; set; }
            public int Incoming { get; set; }
            public int Outgoing { get; set; }
        }

        public class CheckRatings
        {
            public int Id { get; set; }
            public string En { get; set; }
        }

        public class Days
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
            public int Element { get; set; }
        }

        public class Elements
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
        }

        public class Emotes
        {
            public int Id { get; set; }
            public string Command { get; set; }
            public string Alternative { get; set; }
        }

        public class Encumbrance
        {
            public int Id { get; set; }
            public string En { get; set; }
        }

        public class Items
        {
            public int Id { get; set; }
            public int Flags { get; set; }
            public int Stack { get; set; }
            public int Type { get; set; }
            public int Targets { get; set; }
            public string Category { get; set; }
            public string En { get; set; }
            public string Enl { get; set; }
            public string Ja { get; set; }
            public string Jal { get; set; }
            public float CastTime { get; set; }
            public int Level { get; set; }
            public int Slots { get; set; }
            public int Races { get; set; }
            public int Jobs { get; set; }
            public int MaxCharges { get; set; }
            public int CastDelay { get; set; }
            public int RecastDelay { get; set; }
            public int ShieldSize { get; set; }
            public int Damage { get; set; }
            public int Delay { get; set; }
            public int Skill { get; set; }
            public int ItemLevel { get; set; }
            public int SuperiorLevel { get; set; }
        }

        public class ItemDescriptions
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
        }

        public class Jobs
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Ens { get; set; }
            public string Ja { get; set; }
            public string Jas { get; set; }
        }

        public class JobAbilities
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public int Element { get; set; }
            public int IconId { get; set; }
            public int MpCost { get; set; }
            public int RecastId { get; set; }
            public int Targets { get; set; }
            public int TpCost { get; set; }
            public int Duration { get; set; }
            public int Range { get; set; }
            public string Prefix { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
        }

        public class JobPoints
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
            public string Endesc { get; set; }
            public string Jadesc { get; set; }
        }

        public class JobTraits
        {
            public int Id { get; set; }
            public int Element { get; set; }
            public int IconId { get; set; }
            public int Targets { get; set; }
            public int Range { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
        }

        public class KeyItems
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
            public string Category { get; set; }
        }

        public class MeritPoints
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
            public string Endesc { get; set; }
            public string Jadesc { get; set; }
        }

        public class MonsterAbilities
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
            public int Element { get; set; }
            public int IconId { get; set; }
            public int Targets { get; set; }
            public int TpCost { get; set; }
            public int MonsterLevel { get; set; }
            public int Range { get; set; }
            public string Prefix { get; set; }
        }

        public class Monstrosity
        {
            public int Id { get; set; }
            public string TpMoves { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
        }

        public class MoonPhases
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
        }

        public class Mounts
        {
            public int Id { get; set; }
            public string En { get; set; }
            public int IconId { get; set; }
            public string Ja { get; set; }
            public string Endesc { get; set; }
            public string Jadesc { get; set; }
            public string Prefix { get; set; }
        }

        public class Races
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
            public string Gender { get; set; }
        }

        public class Regions
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
        }

        public class Servers
        {
            public int Id { get; set; }
            public string En { get; set; }
        }

        public class Skills
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
            public string Category { get; set; }
        }

        public class Slots
        {
            public int Id { get; set; }
            public string En { get; set; }
        }

        public class Spells
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public int Element { get; set; }
            public int Targets { get; set; }
            public int Skill { get; set; }
            public int MpCost { get; set; }
            public float CastTime { get; set; }
            public float Recast { get; set; }
            public string Levels { get; set; }
            public int RecastId { get; set; }
            public int IconIdNq { get; set; }
            public int IconId { get; set; }
            public int Requirements { get; set; }
            public int Range { get; set; }
            public string Prefix { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
            public bool Unlearnable { get; set; }
            public int Duration { get; set; }
        }

        public class Statuses
        {
            public int Id { get; set; }
            public string En { get; set; }
        }

        public class SynthRanks
        {
            public int Id { get; set; }
            public string En { get; set; }
        }

        public class Titles
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
        }

        public class WeaponSkills
        {
            public int Id { get; set; }
            public string SkillchainA { get; set; }
            public int Element { get; set; }
            public int IconId { get; set; }
            public int Skill { get; set; }
            public string SkillchainC { get; set; }
            public int Targets { get; set; }
            public string SkillchainB { get; set; }
            public int Range { get; set; }
            public string Prefix { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
        }

        public class Weather
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Ja { get; set; }
            public int Element { get; set; }
            public int Intensity { get; set; }
        }

        public class Zones
        {
            public int Id { get; set; }
            public string En { get; set; }
            public string Search { get; set; }
            public string Ja { get; set; }
        }
    }

    public class Resource
    {
        public Resources.ResourceType ResourceType { get; set; }
        public string Id { get; set; }
        public string En { get; set; }
        public string Ja { get; set; }
        public string ActionId { get; set; }
        public string Color { get; set; }
        public string Suffix { get; set; }
        public string Prefix { get; set; }
        public string Access { get; set; }
        public string Command { get; set; }
        public string Equippable { get; set; }
        public string Enl { get; set; }
        public string Jal { get; set; }
        public string Incoming { get; set; }
        public string Outgoing { get; set; }
        public string Element { get; set; }
        public string Alternative { get; set; }
        public string Flags { get; set; }
        public string Stack { get; set; }
        public string Type { get; set; }
        public string Targets { get; set; }
        public string Category { get; set; }
        public string CastTime { get; set; }
        public string Level { get; set; }
        public string Slots { get; set; }
        public string Races { get; set; }
        public string Jobs { get; set; }
        public string MaxCharges { get; set; }
        public string CastDelay { get; set; }
        public string RecastDelay { get; set; }
        public string ShieldSize { get; set; }
        public string Damage { get; set; }
        public string Delay { get; set; }
        public string Skill { get; set; }
        public string ItemLevel { get; set; }
        public string SuperiorLevel { get; set; }
        public string Ens { get; set; }
        public string Jas { get; set; }
        public string IconId { get; set; }
        public string MpCost { get; set; }
        public string RecastId { get; set; }
        public string TpCost { get; set; }
        public string Duration { get; set; }
        public string Range { get; set; }
        public string Endesc { get; set; }
        public string Jadesc { get; set; }
        public string MonsterLevel { get; set; }
        public string TpMoves { get; set; }
        public string Gender { get; set; }
        public string Recast { get; set; }
        public string Levels { get; set; }
        public string IconIdNq { get; set; }
        public string Requirements { get; set; }
        public string Unlearnable { get; set; }
        public string SkillchainA { get; set; }
        public string SkillchainC { get; set; }
        public string SkillchainB { get; set; }
        public string Intensity { get; set; }
        public string Search { get; set; }

        public object As(Type type)
        {
            object resource = Activator.CreateInstance(type);
            ResourceMapper.Map(this, resource);
            return resource;
        }

        public T As<T>()
        {
            return (T)As(typeof(T));
        }
    }
}
