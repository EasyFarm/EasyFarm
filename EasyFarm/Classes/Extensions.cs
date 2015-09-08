/*///////////////////////////////////////////////////////////////////
Copyright (C) <Zerolimits>

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

namespace EasyFarm.Classes
{
    public static class Extensions
    {
        public static IEnumerable<T> RepeatsN<T>(this IEnumerable<T> values, int count)
        {
            return values.GroupBy(x => x)
                .Where(x => x.Count() >= count)
                .SelectMany(x => Enumerable.Repeat(x.Key, x.Count()));
        }
    }
}