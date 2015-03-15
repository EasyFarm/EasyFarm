
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

using Parsing.Abilities;
using Parsing.Mapping;
using Parsing.Types;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Parsing.Augmenting
{
    public class TargetTypeAugmenter : SpecializedTypeAugmenter<TargetType>
    {
        public TargetTypeAugmenter(string attributeName, string variableName) :
            base(attributeName, variableName)
        {
            _mappers.Add(new ObjectMapper<string, TargetType>("Corpse", TargetType.Corpse));
            _mappers.Add(new ObjectMapper<string, TargetType>("Enemy", TargetType.Enemy));
            _mappers.Add(new ObjectMapper<string, TargetType>("NPC", TargetType.NPC));
            _mappers.Add(new ObjectMapper<string, TargetType>("Ally", TargetType.Ally));
            _mappers.Add(new ObjectMapper<string, TargetType>("Party", TargetType.Party));
            _mappers.Add(new ObjectMapper<string, TargetType>("Player", TargetType.Player));
            _mappers.Add(new ObjectMapper<string, TargetType>("Self", TargetType.Self));
        }        
    }
}