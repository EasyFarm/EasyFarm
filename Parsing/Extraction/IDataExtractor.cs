
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

namespace Parsing.Extraction
{
    /// <summary>
    /// Defines how data can be extracted from objects. 
    /// </summary>
    /// <typeparam name="TElement">The object to extract data from. </typeparam>
    /// <typeparam name="TData">The data returned from extraction. </typeparam>
    public interface IDataExtractor<TElement, TData>
    {
        /// <summary>
        /// Checks to see if we can extract data. 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool IsExtractable(TElement data);

        /// <summary>
        /// Extract the data from the given object. 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        TData ExtractData(TElement data);
    }
}
