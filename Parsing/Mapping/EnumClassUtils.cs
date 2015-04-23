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

using System;

namespace Parsing.Mapping
{
    /// This class is thanks to these two great people. The only thing I've done is 
    /// patchwork the two methods together. 
    /// 
    /// Author: James Michael Hare
    /// Source: http://geekswithblogs.net/BlackRabbitCoder/archive/2010/12/09/c.net-little-wonders-fun-with-enum-methods.aspx
    /// Author: Julien Lebosquain 
    /// Source: http://stackoverflow.com/questions/79126/create-generic-method-constraining-t-to-an-enum
    public abstract class EnumClassUtils
    {
        /// <summary>
        ///     Combine generic enums together.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static TType SetFlag<TType>(TType value, TType flags)
        {
            if (!value.GetType().IsEquivalentTo(typeof (TType)))
            {
                throw new ArgumentException("Failed to combine enums generically. ");
            }

            return (TType) Enum.ToObject(typeof (TType), Convert.ToUInt64(value) | Convert.ToUInt64(flags));
        }
    }
}