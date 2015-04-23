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
            _mappers.Add(new ObjectMapper<string, ElementType>("All", ElementType.All));
            _mappers.Add(new ObjectMapper<string, ElementType>("Any", ElementType.Any));
            _mappers.Add(new ObjectMapper<string, ElementType>("Dark", ElementType.Dark));
            _mappers.Add(new ObjectMapper<string, ElementType>("Earth", ElementType.Earth));
            _mappers.Add(new ObjectMapper<string, ElementType>("Fire", ElementType.Fire));
            _mappers.Add(new ObjectMapper<string, ElementType>("Ice", ElementType.Ice));
            _mappers.Add(new ObjectMapper<string, ElementType>("Light", ElementType.Light));
            _mappers.Add(new ObjectMapper<string, ElementType>("None", ElementType.None));
            _mappers.Add(new ObjectMapper<string, ElementType>("NonElemental", ElementType.NonElemental));
            _mappers.Add(new ObjectMapper<string, ElementType>("Thunder", ElementType.Thunder));
            _mappers.Add(new ObjectMapper<string, ElementType>("Trigger", ElementType.trigger));
            _mappers.Add(new ObjectMapper<string, ElementType>("Water", ElementType.Water));
            _mappers.Add(new ObjectMapper<string, ElementType>("Wind", ElementType.Wind));
        }
    }
}