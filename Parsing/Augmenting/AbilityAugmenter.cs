
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

namespace Parsing.Augmenting
{
    /// <summary>
    /// Augments abilities with XElement attribute values by reflecting field 
    /// and property members by name. 
    /// </summary>
    public class AbilityAugmenter<TType> : ResourceValueAugmenter<Ability, TType>
    {
        /// <summary>
        /// Store the attribute's name in [_attributeName]
        /// </summary>
        /// <param name="attributeName"></param>
        public AbilityAugmenter(string attributeName, string variableName)
            : base(attributeName, variableName) { }
    }
}
