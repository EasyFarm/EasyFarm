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

using System.Collections.Generic;
using System.Xml.Linq;
using Parsing.Abilities;
using Parsing.Mapping;

namespace Parsing.Augmenting
{
    public class SpecializedTypeAugmenter<TType> : AbilityAugmenter<Ability>
    {
        /// <summary>
        ///     A list of mappers that map strings to other objects.
        /// </summary>
        protected List<IObjectMapper<string, TType>> Mappers =
            new List<IObjectMapper<string, TType>>();

        public SpecializedTypeAugmenter(string attributeName, string variableName) :
            base(attributeName, variableName)
        {
            // Our default implementation for the specialized mapper that handles the 
            // flags situation for enums where an enum is a combination of multiple
            // states. 
            Mapper = new MultiValueEnumMapper<TType>(Mappers);
        }

        /// <summary>
        ///     Maps a string to a list of potential objects.
        /// </summary>
        public IObjectMapper<string, TType> Mapper { get; set; }

        /// <summary>
        ///     Augment the object with the extracted and converted
        ///     enum data.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="ability"></param>
        public override void Augment(XElement element, Ability ability)
        {
            // If we can't augment, return. 
            if (!CanAugment(element)) return;

            // Can't extract the data. 
            if (!Extractor.IsExtractable(element)) return;

            // Extract the data. 
            var value = Extractor.ExtractData(element);

            // Get all AbilityType mappings for that command. 
            if (Mapper.IsMapped(value))
            {
                // Get the first mapped object. 
                var mapped = Mapper.GetMapping(value);

                // Augment the ability with the data. 
                AugmentObject(ability, mapped);
            }
        }
    }
}