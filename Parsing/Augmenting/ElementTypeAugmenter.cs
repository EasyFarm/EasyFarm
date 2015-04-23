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
    public class ElementTypeAugmenter : SpecializedTypeAugmenter<ElementType>
    {
        public ElementTypeAugmenter(string attributeName, string variableName) :
            base(attributeName, variableName)
        {
            Mappers.Add(new ObjectMapper<string, ElementType>("All", ElementType.All));
            Mappers.Add(new ObjectMapper<string, ElementType>("Any", ElementType.Any));
            Mappers.Add(new ObjectMapper<string, ElementType>("Dark", ElementType.Dark));
            Mappers.Add(new ObjectMapper<string, ElementType>("Earth", ElementType.Earth));
            Mappers.Add(new ObjectMapper<string, ElementType>("Fire", ElementType.Fire));
            Mappers.Add(new ObjectMapper<string, ElementType>("Ice", ElementType.Ice));
            Mappers.Add(new ObjectMapper<string, ElementType>("Light", ElementType.Light));
            Mappers.Add(new ObjectMapper<string, ElementType>("None", ElementType.None));
            Mappers.Add(new ObjectMapper<string, ElementType>("NonElemental", ElementType.NonElemental));
            Mappers.Add(new ObjectMapper<string, ElementType>("Thunder", ElementType.Thunder));
            Mappers.Add(new ObjectMapper<string, ElementType>("Trigger", ElementType.Trigger));
            Mappers.Add(new ObjectMapper<string, ElementType>("Water", ElementType.Water));
            Mappers.Add(new ObjectMapper<string, ElementType>("Wind", ElementType.Wind));
        }
    }
}