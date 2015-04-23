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
using System.Linq;

namespace Parsing.Mapping
{
    /// <summary>
    ///     Maps comma separated values into strongly typed enum objects.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    public abstract class BaseEnumMapper<TType> :
        EnumClassUtils<TType>,
        IObjectMapper<string, TType>
    {
        /// <summary>
        ///     The internal list of mappers.
        /// </summary>
        protected IEnumerable<IObjectMapper<string, TType>> _mappers;

        /// <summary>
        ///     Save a list of mappers to compare values against.
        /// </summary>
        /// <param name="mappers"></param>
        public BaseEnumMapper(IEnumerable<IObjectMapper<string, TType>> mappers)
        {
            _mappers = mappers;
        }

        /// <summary>
        ///     Check to see if any mappers can map the
        ///     string separated enum flag values.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract bool IsMapped(string obj);

        /// <summary>
        ///     Return an enum with flags indicated by the string
        ///     object obj.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract TType GetMapping(string obj);

        /// <summary>
        ///     Split and clean the string data for parsing.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected IEnumerable<string> SplitData(string obj)
        {
            return obj.Split(',')
                .Select(x => x.Trim())
                .ToList();
        }
    }
}