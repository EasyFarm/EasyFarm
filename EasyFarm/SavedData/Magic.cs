
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

namespace EasyFarm.UserSettings
{
    public class Magic
    {
        public bool Enabled = false;
        public int High = 100;
        public int Low = 50;

        public bool ShouldRest(int magic, Status status)
        {
            return (Enabled && (IsMagicLow(magic) || !IsMagicHigh(magic) && status == Status.Healing));
        }

        public bool IsMagicLow(int magic)
        {
            return magic <= Low;
        }

        public bool IsMagicHigh(int magic)
        {
            return magic >= High;
        }
    }
}
