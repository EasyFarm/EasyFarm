
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
            base(attributeName, variableName) { }

        /// <summary>
        /// A list of mappers that map strings to other objects.  
        /// </summary>
        protected List<IObjectMapper<string, TType>> _mappers =
            new List<IObjectMapper<string, TType>>();

        /// <summary>
        /// Maps a string to a list of potential objects. 
        /// </summary>
        private EnumObjectMapper<TType> _mapper;

        public override void Augment(XElement element, Ability ability)
        {
            // If we can't augment, return. 
            if (!CanAugment(element)) return;

            // Can't extract the data. 
            if (!_extractor.IsExtractable(element)) return;

            // Extract the data. 
            var value = _extractor.ExtractData(element);

            // Store all the mappers in the master map object. 
            _mapper = new EnumObjectMapper<TType>(_mappers);

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
