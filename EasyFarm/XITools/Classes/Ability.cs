
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
using ZeroLimits.XITool.Enums;

namespace ZeroLimits.XITool.Classes
{
    /// <summary>
    /// An action to be used on a target unit or player.
    /// Could be a spell or an ability.
    /// </summary>
    public class Ability
    {
        /// <summary>
        /// Set default values to avoid null exception errors. 
        /// </summary>
        public Ability() 
        {
            this.ActionType = Enums.ActionType.None;
            this.Alias = String.Empty;
            this.CastTime = 0;
            this.Element = String.Empty;
            this.ID = 0;
            this.Index = 0;
            this.MPCost = 0;
            this.Name = String.Empty;
            this.Postfix = String.Empty;
            this.Prefix = String.Empty;
            this.Recast = 0;
            this.Skill = String.Empty;
            this.Targets = String.Empty;
            this.TPCost = 0;
            this.Type = String.Empty;
        }

        /// <summary>
        /// The ability ID in its own resource file. 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Unique number that allows for retrieving ability recast times. 
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        /// How much magic the ability costs. 
        /// </summary>
        public int? MPCost { get; set; }

        /// <summary>
        /// How much tp the ability costs.
        /// </summary>
        public int? TPCost { get; set; }

        /// <summary>
        /// How long the ability takes to cast. 
        /// </summary>
        public double? CastTime { get; set; }

        /// <summary>
        /// How long to wait before the ability can be casted again. 
        /// </summary>
        public double? Recast { get; set; }

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
        public string Prefix { get; set; }

        /// <summary>
        /// The name of the ability.
        /// </summary>
        public string Name { get; set; }

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
        public string Type { get; set; }

        /// <summary>
        /// The element type of the ability:
        /// Light, Wind, Earth, Water, Ice, Fire, 
        /// Thunder, Dark, NonElemental, None, 
        /// trigger, Any, All
        /// 
        /// Example: Light, Dark
        /// </summary>
        public string Element { get; set; }

        /// <summary>
        /// The target types can the ability be used on. 
        /// It can be a combination of these fields: 
        /// Self, Player, Party, Ally, NPC, Enemy, Corpse
        /// 
        /// Example: Self, Party
        /// </summary>
        public string Targets { get; set; }

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
        public string Skill { get; set; }

        /// <summary>
        /// The alias for the ability. Not every ability 
        /// has an alias. 
        /// 
        /// Example: Cure's alias is c1
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// The target for the ability. 
        /// Example: <t>, <st>, <stnpc>
        /// </summary>
        public string Postfix { get; set; }

        /// <summary>
        /// The ability's action type. This is based on 
        /// the Prefix but is more strongly typed by being 
        /// represented by an enum; no string comparisons. 
        /// </summary>
        public ActionType ActionType { get; set; }

        /// <summary>
        /// Indicates whether we created a valid ability or not. 
        /// </summary>
        public bool IsValidName { get { return !string.IsNullOrEmpty(Name); } }

        /// <summary>
        /// Translates Targets into PostFixes for use in 
        /// solo applications. It translates Target references 
        /// as so: self = <me>; enemy = <t>. It also properly 
        /// formats ranged attack command strings. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // If it was intended to work on use, 
            // set it to cast on us
            if (Targets.ToLower().Contains("self"))
                Postfix = "<me>";
            else if (Targets.ToLower().Contains("enemy"))
                Postfix = "<t>";

            // If it was a ranged attack, use the ranged attack syntax
            if (Prefix == "/range")
                return Prefix + " " + Postfix;
            // Use the spell/ability syntax.
            else
                return Prefix + " \"" + Name + "\" " + Postfix;
        }
    }
}