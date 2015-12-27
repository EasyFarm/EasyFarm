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
    [Flags]
    public enum TargetType
    {
        Unknown = 0x0000,
        Self = 0x0001,
        Player = 0x0002,
        Party = 0x0004,
        Ally = 0x0008,
        Npc = 0x0016,
        Enemy = 0x0032,
        Corpse = 0x0064
    }
}