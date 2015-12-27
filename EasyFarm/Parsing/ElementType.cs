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

namespace EasyFarm.Parsing
{
    /// <summary>
    ///     The elemental type for the ability.
    /// </summary>
    [Flags]
    public enum ElementType
    {
        Unknown = 0x0000,
        Light = 0x0001,
        Wind = 0x0002,
        Earth = 0x0004,
        Water = 0x0008,
        Ice = 0x0016,
        Fire = 0x0032,
        Thunder = 0x0064,
        Dark = 0x0128,
        NonElemental = 0x0256,
        None = 0x0512,
        Trigger = 0x1024,
        Any = 0x2048,
        All = 0x4096
    }
}