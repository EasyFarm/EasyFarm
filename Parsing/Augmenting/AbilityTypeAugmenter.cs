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
    /// <summary>
    ///     Augments abilities with AbilityType objects.
    /// </summary>
    public class AbilityTypeAugmenter : SpecializedTypeAugmenter<AbilityType>
    {
        public AbilityTypeAugmenter(string attributeName, string variableName) :
            base(attributeName, variableName)
        {
            // Create mappings from ffxi command to AbilityType. 
            Mappers.Add(new ObjectMapper<string, AbilityType>("/jobability", AbilityType.Jobability));
            Mappers.Add(new ObjectMapper<string, AbilityType>("/echo", AbilityType.Echo));
            Mappers.Add(new ObjectMapper<string, AbilityType>("/magic", AbilityType.Magic));
            Mappers.Add(new ObjectMapper<string, AbilityType>("/monsterskill", AbilityType.Monsterskill));
            Mappers.Add(new ObjectMapper<string, AbilityType>("/ninjutsu", AbilityType.Ninjutsu));
            Mappers.Add(new ObjectMapper<string, AbilityType>("/pet", AbilityType.Pet));
            Mappers.Add(new ObjectMapper<string, AbilityType>("/range", AbilityType.Range));
            Mappers.Add(new ObjectMapper<string, AbilityType>("/song", AbilityType.Song));
            Mappers.Add(new ObjectMapper<string, AbilityType>("/trigger", AbilityType.Trigger));
            Mappers.Add(new ObjectMapper<string, AbilityType>("/weaponskill", AbilityType.Weaponskill));
        }
    }
}