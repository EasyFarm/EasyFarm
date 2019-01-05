// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
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
using System.Linq;
using EliteMMO.API;

namespace EasyFarm.Parsing
{
    public class AbilityMapper
    {
        public Ability Map(EliteAPI.IAbility ability)
        {
            return new Ability
            {
                English = ability.Name?.FirstOrDefault() ?? "",
                Distance = ability.Range,
                Index = ability.TimerID,
                Prefix = ability.TimerID == 900 ? "/weaponskill" : "/jobability",
                TpCost = ability.TimerID == 900 ? 1000 : ability.TP,
                TargetType = (TargetType) ability.ValidTargets,
                AbilityType = GetAbilityType(ability)
            };
        }

        private AbilityType GetAbilityType(EliteAPI.IAbility ability)
        {
            EliteMMO.API.AbilityType abilityType = (EliteMMO.API.AbilityType)ability.Type;

            switch (abilityType)
            {
                case EliteMMO.API.AbilityType.Job:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.General:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.BloodPactRage:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Corsair:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.CorsairShot:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.BloodPactWard:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Samba:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Waltz:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Step:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Florish1:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Scholar:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Jig:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Flourish2:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Flourish3:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Pet:
                    return AbilityType.Pet;
                case EliteMMO.API.AbilityType.Monster:
                    return AbilityType.Monsterskill;
                case EliteMMO.API.AbilityType.Weapon:
                    return AbilityType.Weaponskill;
                case EliteMMO.API.AbilityType.Weaponskill:
                    return AbilityType.Weaponskill;
                case EliteMMO.API.AbilityType.Trait:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Rune:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Ward:
                    return AbilityType.Jobability;
                case EliteMMO.API.AbilityType.Effusion:
                    return AbilityType.Jobability;
            }

            return AbilityType.Unknown;
        }
    }
}