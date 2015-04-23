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

namespace Parsing.Mapping
{
    /// <summary>
    ///     Maps comma separated values into strongly typed enum objects.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    public class MultiValueEnumMapper<TType> : BaseEnumMapper<TType>
    {
        /// <summary>
        ///     Save a list of mappers to compare values against.
        /// </summary>
        /// <param name="mappers"></param>
        public MultiValueEnumMapper(IEnumerable<IObjectMapper<string, TType>> mappers)
            : base(mappers)
        {
        }

        /// <summary>
        ///     Check to see if any mappers can map the
        ///     string separated enum flag values.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool IsMapped(string obj)
        {
            foreach (var data in SplitData(obj))
            {
                foreach (var mapper in _mappers)
                {
                    if (mapper.IsMapped(data))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     Return an enum with flags indicated by the string
        ///     object obj.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override TType GetMapping(string obj)
        {
            var value = default(TType);

            var flags = new List<TType>();

            // Map all elements to their proper element
            // and combine them together into one ElementType object. 
            foreach (var data in SplitData(obj))
            {
                foreach (var mapper in _mappers)
                {
                    if (mapper.IsMapped(data))
                    {
                        value = SetFlag(value, mapper.GetMapping(data));
                    }
                }
            }

            return value;
        }
    }
}