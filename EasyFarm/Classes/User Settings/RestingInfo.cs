
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
            this.Magic = new Magic();
            this.Health = new Health();

            SetDefaults();
        }

        private void SetDefaults()
        {
            Health.Low = 0;
            Magic.Low = 0;
            Health.High = 0;
            Magic.High = 0;
            Health.Enabled = false;
            Magic.Enabled = false;
        }

        /// <summary>
        /// Data for HP
        /// </summary>
        public Health Health { get; set; }
        
        /// <summary>
        /// Data for MP
        /// </summary>
        public Magic Magic { get; set; }      
    }
}
