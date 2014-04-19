
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

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace EasyFarm.Classes
{
    /// <summary>
    /// An action to be used on a target unit or player.
    /// Could be a spell or an ability.
    /// </summary>
    public class Ability
    {
        public Ability() { }
        public int ID { get; set; }
        public int Index { get; set; }
        public int MPCost { get; set; }
        public int? TPCost { get; set; }
        public double CastTime { get; set; }
        public double Recast { get; set; }
        public string Prefix { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Element { get; set; }
        public string Targets { get; set; }
        public string Skill { get; set; }
        public string Alias { get; set; }
        public string Postfix { get; set; }
        public bool IsSpell { get; set;}
        public bool IsAbility { get; set; }
        public bool IsValidName { get { return !string.IsNullOrWhiteSpace(Name); } }


        /// <summary>
        /// Returns the command to execute the ability or spell
        ///      /ma "Dia" <t>
        ///      /ja "Provoke" <t>
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
