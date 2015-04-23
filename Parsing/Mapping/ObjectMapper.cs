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

namespace Parsing.Mapping
{
    /// <summary>
    ///     Generic mapper for data that uses equals to map equal references on
    ///     reference types or equal values on value types.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TData"></typeparam>
    public class ObjectMapper<TObject, TData> : IObjectMapper<TObject, TData>
    {
        /// <summary>
        ///     The data to return on successful mapping.
        /// </summary>
        private readonly TData _data;

        /// <summary>
        ///     The object to map against.
        /// </summary>
        private readonly TObject _obj;

        /// <summary>
        ///     Store the data and object to map.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="data"></param>
        public ObjectMapper(TObject obj, TData data)
        {
            _obj = obj;
            _data = data;
        }

        /// <summary>
        ///     Checks if there's a mapping from the given object
        ///     to our internal object.
        /// </summary>
        /// <param name="obj">The object to check the mapping against. </param>
        /// <returns></returns>
        public bool IsMapped(TObject obj)
        {
            return _obj.Equals(obj);
        }

        /// <summary>
        ///     Gets the data if the given object maps.
        /// </summary>
        /// <param name="obj">The object to check the mapping againsts. </param>
        /// <returns></returns>
        public TData GetMapping(TObject obj)
        {
            // Return the data if there's a mapping. 
            if (IsMapped(obj)) return _data;
            return default(TData);
        }
    }
}