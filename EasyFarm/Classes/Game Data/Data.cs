
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
using System.Threading.Tasks;

namespace EasyFarm.Classes.Game_Data
{
    public class Data
    {
        GameEngine _engine;
        PlayerData _pdata;
        TargetData _tdata;

        public Data(ref GameEngine engine)
        {
            _engine = engine;
            _pdata = new PlayerData(ref engine);
            _tdata = new TargetData(ref engine);
        }

        public PlayerData Player 
        {
            get { return _pdata; }
            set { this._pdata = value; }
        }

        public TargetData Target
        {
            get { return _tdata; }
            set { this._tdata = value; }
        }
    }
}
