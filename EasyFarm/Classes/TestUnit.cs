
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
using ZeroLimits.XITool.Interfaces;

namespace ZeroLimits.XITool.Test
{
    public class TestUnit : IUnit
    {
        public int ID { get; set; }

        public int ClaimedID { get; set; }

        public double Distance { get; set; }

        public FFACETools.FFACE.Position Position { get; set; }

        public short HPPCurrent { get; set; }

        public bool IsActive { get; set; }

        public bool IsClaimed { get; set; }

        public bool IsRendered { get; set; }

        public string Name { get; set; }

        public byte NPCBit { get; set; }

        public NPCType NPCType { get; set; }

        public int PetID { get; set; }

        public float PosH { get; set; }

        public float PosX { get; set; }

        public float PosY { get; set; }

        public float PosZ { get; set; }

        public byte[] RawData { get; set; }

        public Status Status { get; set; }

        public short TPCurrent { get; set; }

        public bool MyClaim { get; set; }

        public bool HasAggroed { get; set; }

        public bool IsDead { get; set; }

        public bool PartyClaim { get; set; }

        public double YDifference { get; set; }
    }
}
