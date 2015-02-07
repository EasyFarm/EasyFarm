
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

using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroLimits.XITool.Interfaces
{
    public interface IUnit
    {
        int ID { get; set; }

        int ClaimedID { get; }

        double Distance { get; }

        FFACE.Position Position { get; }

        short HPPCurrent { get; }

        bool IsActive { get; }

        bool IsClaimed { get; }

        bool IsRendered { get; }

        string Name { get; }

        byte NPCBit { get; }

        NPCType NPCType { get; }

        int PetID { get; }

        float PosH { get; }

        float PosX { get; }

        float PosY { get; }

        float PosZ { get; }

        byte[] RawData { get; }

        Status Status { get; }

        short TPCurrent { get; }

        bool MyClaim { get; }

        bool HasAggroed { get; }

        bool IsDead { get; }

        bool PartyClaim { get; }

        double YDifference { get; }

        string ToString();
    }
}
