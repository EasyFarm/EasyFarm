
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Parsing.Augmenting
{
    public class SpecializedTypeAugmenter<TType> : AbilityAugmenter<Ability>
    {
        public SpecializedTypeAugmenter(string attributeName, string variableName) :
            base(attributeName, variableName) 
        {
            // Our default implementation for the specialized mapper that handles the 
            // flags situation for enums where an enum is a combination of multiple
            // states. 
            _mapper = new MultiValueEnumMapper<TType>(_mappers);
        }

        /// <summary>
        /// A list of mappers that map strings to other objects.  
        /// </summary>
        protected List<IObjectMapper<string, TType>> _mappers =
            new List<IObjectMapper<string, TType>>();

        /// <summary>
        /// Maps a string to a list of potential objects. 
        /// </summary>
        public IObjectMapper<string, TType> _mapper 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Augment the object with the extracted and converted
        /// enum data. 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="ability"></param>
        public override void Augment(XElement element, Ability ability)
        {
            // If we can't augment, return. 
            if (!CanAugment(element)) return;

            // Can't extract the data. 
            if (!_extractor.IsExtractable(element)) return;

            // Extract the data. 
            var value = _extractor.ExtractData(element);

            // Get all AbilityType mappings for that command. 
            if (_mapper.IsMapped(value))
            {
                // Get the first mapped object. 
                var mapped = _mapper.GetMapping(value);

                // Augment the ability with the data. 
                AugmentObject(ability, mapped);
            }
        }
    }
}
