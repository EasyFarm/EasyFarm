
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

namespace Parsing.Augmenting
{
    /// <summary>
    /// The interface defines how data is augmented or 
    /// assignd data. 
    /// </summary>
    /// <typeparam name="TElement">Element to extract data from. </typeparam>
    /// <typeparam name="TObject">The object to augment with data. </typeparam>
    public interface IObjectAugmenter<TElement, TObject>
    {
        /// <summary>
        /// Determines whether we can extract data from the object. 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool CanAugment(TElement element);

        /// <summary>
        /// Assigns the element [T] to a field in [R]. 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        void Augment(TElement element, TObject obj);
    }
}
