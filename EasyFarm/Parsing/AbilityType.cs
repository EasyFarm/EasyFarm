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
    ///     Represents the command used to trigger the action.
    /// </summary>
    [Flags]
    public enum AbilityType
    {
        Unknown = 0x0000,
        Magic = 0x0001,
        Ninjutsu = 0x0002,
        Song = 0x0004,
        Trigger = 0x0008,
        Weaponskill = 0x0016,
        Range = 0x0032,
        Echo = 0x0064,
        Jobability = 0x0128,
        Pet = 0x0256,
        Monsterskill = 0x0512
    }
}