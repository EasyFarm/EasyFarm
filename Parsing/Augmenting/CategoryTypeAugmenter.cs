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

using Parsing.Mapping;
using Parsing.Types;

namespace Parsing.Augmenting
{
    public class CategoryTypeAugmenter : SpecializedTypeAugmenter<CategoryType>
    {
        public CategoryTypeAugmenter(string attributeName, string variableName) :
            base(attributeName, variableName)
        {
            _mappers.Add(new ObjectMapper<string, CategoryType>("WhiteMagic", CategoryType.WhiteMagic));
            _mappers.Add(new ObjectMapper<string, CategoryType>("BlackMagic", CategoryType.BlackMagic));
            _mappers.Add(new ObjectMapper<string, CategoryType>("SummonerPact", CategoryType.SummonerPact));
            _mappers.Add(new ObjectMapper<string, CategoryType>("Ninjustsu", CategoryType.Ninjustsu));
            _mappers.Add(new ObjectMapper<string, CategoryType>("Geomancy", CategoryType.Geomancy));
            _mappers.Add(new ObjectMapper<string, CategoryType>("BlueMagic", CategoryType.BlueMagic));
            _mappers.Add(new ObjectMapper<string, CategoryType>("BardSong", CategoryType.BardSong));
            _mappers.Add(new ObjectMapper<string, CategoryType>("Trust", CategoryType.Trust));
            _mappers.Add(new ObjectMapper<string, CategoryType>("WeaponSkill", CategoryType.WeaponSkill));
            _mappers.Add(new ObjectMapper<string, CategoryType>("Misc", CategoryType.Misc));
            _mappers.Add(new ObjectMapper<string, CategoryType>("JobAbility", CategoryType.JobAbility));
            _mappers.Add(new ObjectMapper<string, CategoryType>("PetCommand", CategoryType.PetCommand));
            _mappers.Add(new ObjectMapper<string, CategoryType>("CorsairRoll", CategoryType.CorsairRoll));
            _mappers.Add(new ObjectMapper<string, CategoryType>("CorsairShot", CategoryType.CorsairShot));
            _mappers.Add(new ObjectMapper<string, CategoryType>("Samba", CategoryType.Samba));
            _mappers.Add(new ObjectMapper<string, CategoryType>("Waltz", CategoryType.Waltz));
            _mappers.Add(new ObjectMapper<string, CategoryType>("Jig", CategoryType.Jig));
            _mappers.Add(new ObjectMapper<string, CategoryType>("Step", CategoryType.Step));
            _mappers.Add(new ObjectMapper<string, CategoryType>("Flourish1", CategoryType.Flourish1));
            _mappers.Add(new ObjectMapper<string, CategoryType>("Flourish2", CategoryType.Flourish2));
            _mappers.Add(new ObjectMapper<string, CategoryType>("Effusion", CategoryType.Effusion));
            _mappers.Add(new ObjectMapper<string, CategoryType>("Rune", CategoryType.Rune));
            _mappers.Add(new ObjectMapper<string, CategoryType>("Ward", CategoryType.Ward));
            _mappers.Add(new ObjectMapper<string, CategoryType>("BloodPactWard", CategoryType.BloodPactWard));
            _mappers.Add(new ObjectMapper<string, CategoryType>("BloodPactRage", CategoryType.BloodPactRage));
            _mappers.Add(new ObjectMapper<string, CategoryType>("Monster", CategoryType.Monster));
            _mappers.Add(new ObjectMapper<string, CategoryType>("JobTrait", CategoryType.JobTrait));
            _mappers.Add(new ObjectMapper<string, CategoryType>("MonsterSkill", CategoryType.MonsterSkill));
        }
    }
}