
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

using Parsing.Types;

namespace Parsing.Abilities
{
    /// <summary>
    /// An action to be used on a target unit or player.
    /// Could be a spell or an ability.
    /// </summary>
    public class Ability
    {
        public Ability()
        {
            this.AbilityType = AbilityType.Unknown;
            this.Alias = string.Empty;
            this.CastTime = 0;
            this.CategoryType = CategoryType.Unknown;
            this.Distance = 0;
            this.Element = string.Empty;
            this.ElementType = ElementType.Unknown;
            this.English = string.Empty;
            this.ID = 0;
            this.Index = 0;
            this.Japanese = string.Empty;
            this.MPCost = MPCost;
            this.Postfix = string.Empty;
            this.Prefix = string.Empty;
            this.Recast = 0;
            this.Skill = string.Empty;
            this.SkillType = SkillType.Unknown;
            this.Targets = string.Empty;
            this.TargetType = TargetType.Unknown;
            this.TPCost = 0;
            this.Type = string.Empty;
        }

        /// <summary>
        /// The ability ID in its own resource file. 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Unique number that allows for retrieving ability recast times. 
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// The name of the ability in English. 
        /// </summary>
        public string English { get; set; }

        /// <summary>
        /// The name of the ability in Japanese. 
        /// </summary>
        public string Japanese { get; set; }

        /// <summary>
        /// The mp cost for the ability. 
        /// </summary>
        public int MPCost { get; set; }

        /// <summary>
        /// The tp cost for the ablility. 
        /// </summary>
        public int TPCost { get; set; }

        /// <summary>
        /// The alias for the ability. Not every ability 
        /// has an alias. 
        /// 
        /// Example: Cure's alias is c1
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// The first part of the command that tells us what type
        /// of ability it is:
        /// 
        /// Magic: 
        /// /magic, /ninjutsu, /song, /trigger, 
        /// 
        /// Ability: 
        /// /weaponskill, 
        /// /range, /echo, /jobability, /pet, /monsterskill. 
        /// </summary>
        public AbilityType AbilityType { get; set; }

        /// <summary>
        /// String representation for the AbilityType. 
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// The target for the ability. 
        /// Example: <t>, <st>, <stnpc>
        /// </summary>
        public string Postfix { get; set; }

        /// <summary>
        /// How long the ability takes to cast. 
        /// </summary>
        public double CastTime { get; set; }

        /// <summary>
        /// How long to wait before the ability can be casted again. 
        /// </summary>
        public double Recast { get; set; }

        /// <summary>
        /// The max range with which this ability can be used. 
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// The type of ability: 
        /// 
        /// Magic:
        /// WhiteMagic, BlackMagic, SummonerPact, Ninjustsu, Geomancy, 
        /// BlueMagic, BardSong, Trust, trigger, 
        /// 
        /// Ability: 
        /// WeaponSkill, Misc, JobAbility, PetCommand, CorsairRoll, 
        /// CorsairShot, Samba, Waltz, Jig, Step, Flourish1, Flourish2, 
        /// Effusion, Rune, Ward, BloodPactWard, BloodPactRage, Monster,
        /// JobTrait, MonsterSkill
        /// 
        /// Example: BlueMagic
        /// </summary>
        public CategoryType CategoryType { get; set; }

        /// <summary>
        /// String representation for the catergory type. 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The type of skill the ability is: 
        /// 
        /// HealingMagic, DivineMagic, EnfeeblingMagic,
        /// EnhancingMagic, ElementalMagic, DarkMagic, 
        /// SummoningMagic, Ninjutsu, Singing, BlueMagic
        /// Geomancy, ControlTrigger, GenericTrigger, 
        /// ElementalTrigger, CombatTrigger, trigger, 0, 
        /// Ability
        /// </summary>
        public SkillType SkillType { get; set; }

        /// <summary>
        /// String representation of the skill type. 
        /// </summary>
        public string Skill { get; set; }

        /// <summary>
        /// The element type of the ability:
        /// Light, Wind, Earth, Water, Ice, Fire, 
        /// Thunder, Dark, NonElemental, None, 
        /// trigger, Any, All
        /// 
        /// Example: Light, Dark
        /// </summary>
        public ElementType ElementType { get; set; }

        /// <summary>
        /// String representation of the element type. 
        /// </summary>
        public string Element { get; set; }

        /// <summary>
        /// The target types can the ability be used on. 
        /// It can be a combination of these fields: 
        /// Self, Player, Party, Ally, NPC, Enemy, Corpse
        /// 
        /// Example: Self, Party
        /// </summary>
        public TargetType TargetType { get; set; }

        /// <summary>
        /// String representation of the target type. 
        /// </summary>
        public string Targets { get; set; }

        /// <summary>
        /// Indicates whether we created a valid ability or not. 
        /// </summary>
        public bool IsValidName
        {
            get
            {
                return !string.IsNullOrEmpty(English);
            }
        }

        public override string ToString()
        {
            // If it was intended to work on use, 
            // set it to cast on us
            if (TargetType.HasFlag(TargetType.Self))
            {
                Postfix = "<me>";
            }
            else if (TargetType.HasFlag(TargetType.Enemy))
            {
                Postfix = "<t>";
            }

            // If it was a ranged attack, use the ranged attack syntax
            if (AbilityType.HasFlag(AbilityType.Range))
            {
                return Prefix + " " + Postfix;
            }

            // Use the spell/ability syntax.
            else
            {
                return Prefix + " \"" + English + "\" " + Postfix;
            }
        }        
    }
}