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
    ///     Maps a single object to multiple mappings.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TData"></typeparam>
    public class MultiObjectMapper<TObject, TData> : IObjectMultiMapper<TObject, TData>
    {
        /// <summary>
        ///     Collection of mappers to check for mappings.
        /// </summary>
        private readonly IEnumerable<IObjectMapper<TObject, TData>> _mappers;

        public MultiObjectMapper(IEnumerable<IObjectMapper<TObject, TData>> mappers)
        {
            _mappers = mappers;
        }

        /// <summary>
        ///     Check for any mappings
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsMapped(TObject obj)
        {
            foreach (var mapper in _mappers)
            {
                if (mapper.IsMapped(obj))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Return all matching mapped data for the object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<TData> GetMapping(TObject obj)
        {
            var data = new List<TData>();

            // Map all elements to their proper element
            // and combine them together into one ElementType object. 
            foreach (var mapper in _mappers)
            {
                if (mapper.IsMapped(obj))
                {
                    data.Add(mapper.GetMapping(obj));
                }
            }

            return data;
        }
    }
}