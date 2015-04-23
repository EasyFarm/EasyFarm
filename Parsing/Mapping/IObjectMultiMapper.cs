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
    ///     Defines an interface for creating a mapping from one object to another.
    /// </summary>
    /// <typeparam name="TData">
    ///     The data returned when an object is mapped successfully.
    /// </typeparam>
    /// <typeparam name="TObject">
    ///     The object compared to see if there's a mapping.
    /// </typeparam>
    public interface IObjectMultiMapper<in TObject, out TData>
    {
        /// <summary>
        ///     Checks whether there's a mapping from obj to data.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool IsMapped(TObject obj);

        /// <summary>
        ///     Returns the data object if there's a mapping.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IEnumerable<TData> GetMapping(TObject obj);
    }
}