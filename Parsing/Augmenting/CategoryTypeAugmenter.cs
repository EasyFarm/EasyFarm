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
            Mappers.Add(new ObjectMapper<string, CategoryType>("WhiteMagic", CategoryType.WhiteMagic));
            Mappers.Add(new ObjectMapper<string, CategoryType>("BlackMagic", CategoryType.BlackMagic));
            Mappers.Add(new ObjectMapper<string, CategoryType>("SummonerPact", CategoryType.SummonerPact));
            Mappers.Add(new ObjectMapper<string, CategoryType>("Ninjustsu", CategoryType.Ninjustsu));
            Mappers.Add(new ObjectMapper<string, CategoryType>("Geomancy", CategoryType.Geomancy));
            Mappers.Add(new ObjectMapper<string, CategoryType>("BlueMagic", CategoryType.BlueMagic));
            Mappers.Add(new ObjectMapper<string, CategoryType>("BardSong", CategoryType.BardSong));
            Mappers.Add(new ObjectMapper<string, CategoryType>("Trust", CategoryType.Trust));
            Mappers.Add(new ObjectMapper<string, CategoryType>("WeaponSkill", CategoryType.WeaponSkill));
            Mappers.Add(new ObjectMapper<string, CategoryType>("Misc", CategoryType.Misc));
            Mappers.Add(new ObjectMapper<string, CategoryType>("JobAbility", CategoryType.JobAbility));
            Mappers.Add(new ObjectMapper<string, CategoryType>("PetCommand", CategoryType.PetCommand));
            Mappers.Add(new ObjectMapper<string, CategoryType>("CorsairRoll", CategoryType.CorsairRoll));
            Mappers.Add(new ObjectMapper<string, CategoryType>("CorsairShot", CategoryType.CorsairShot));
            Mappers.Add(new ObjectMapper<string, CategoryType>("Samba", CategoryType.Samba));
            Mappers.Add(new ObjectMapper<string, CategoryType>("Waltz", CategoryType.Waltz));
            Mappers.Add(new ObjectMapper<string, CategoryType>("Jig", CategoryType.Jig));
            Mappers.Add(new ObjectMapper<string, CategoryType>("Step", CategoryType.Step));
            Mappers.Add(new ObjectMapper<string, CategoryType>("Flourish1", CategoryType.Flourish1));
            Mappers.Add(new ObjectMapper<string, CategoryType>("Flourish2", CategoryType.Flourish2));
            Mappers.Add(new ObjectMapper<string, CategoryType>("Effusion", CategoryType.Effusion));
            Mappers.Add(new ObjectMapper<string, CategoryType>("Rune", CategoryType.Rune));
            Mappers.Add(new ObjectMapper<string, CategoryType>("Ward", CategoryType.Ward));
            Mappers.Add(new ObjectMapper<string, CategoryType>("BloodPactWard", CategoryType.BloodPactWard));
            Mappers.Add(new ObjectMapper<string, CategoryType>("BloodPactRage", CategoryType.BloodPactRage));
            Mappers.Add(new ObjectMapper<string, CategoryType>("Monster", CategoryType.Monster));
            Mappers.Add(new ObjectMapper<string, CategoryType>("JobTrait", CategoryType.JobTrait));
            Mappers.Add(new ObjectMapper<string, CategoryType>("MonsterSkill", CategoryType.MonsterSkill));
        }
    }
}