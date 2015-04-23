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
    public class TargetTypeAugmenter : SpecializedTypeAugmenter<TargetType>
    {
        public TargetTypeAugmenter(string attributeName, string variableName) :
            base(attributeName, variableName)
        {
            Mappers.Add(new ObjectMapper<string, TargetType>("Corpse", TargetType.Corpse));
            Mappers.Add(new ObjectMapper<string, TargetType>("Enemy", TargetType.Enemy));
            Mappers.Add(new ObjectMapper<string, TargetType>("NPC", TargetType.Npc));
            Mappers.Add(new ObjectMapper<string, TargetType>("Ally", TargetType.Ally));
            Mappers.Add(new ObjectMapper<string, TargetType>("Party", TargetType.Party));
            Mappers.Add(new ObjectMapper<string, TargetType>("Player", TargetType.Player));
            Mappers.Add(new ObjectMapper<string, TargetType>("Self", TargetType.Self));

            // Use the single value enum mapper. 
            Mapper = new SingleValueEnumMapper<TargetType>(Mappers);
        }
    }
}