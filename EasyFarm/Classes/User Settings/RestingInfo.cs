
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013 - 2014>  <Zerolimits>

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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    public class RestingInfo
    {
        public RestingInfo()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            LowHP = 0;
            LowMP = 0;
            HighHP = 0;
            HighMP = 0;
            IsRestingHPEnabled = false;
            IsRestingMPEnabled = false;
        }

        /// <summary>
        /// The value we should rest mp at
        /// </summary>
        public int LowMP { get; set; }

        /// <summary>
        /// The value we can get up from resting mp
        /// </summary>
        public int HighMP { get; set; }

        /// <summary>
        /// The value we can get up from resting hp
        /// </summary>
        public int HighHP { get; set; }

        /// <summary>
        /// The value we should rest hp at.
        /// </summary>
        public int LowHP { get; set; }

        /// <summary>
        /// Should we rest hp at all?
        /// </summary>
        public bool IsRestingHPEnabled { get; set; }

        /// <summary>
        /// Should we rest mp at all?
        /// </summary>
        public bool IsRestingMPEnabled { get; set; }
    }
}
