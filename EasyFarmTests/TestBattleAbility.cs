
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

using EasyFarm.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarmTests.MachineComponentTests
{
    public class TestBattleAbility
    {
        private string _name = string.Empty;
        private bool _enabled = false;
        private bool _isBuff = false;
        private bool _effectWore = false;

        public TestBattleAbility(String name, bool enabled , bool isbuff, bool effectwore)
        {
            _name = name;
            _enabled = enabled;
            _isBuff = isbuff;
            _effectWore = effectwore;
        }

        public string Name 
        {
            get 
            {
                return _name;
            }
        }

        public bool Enabled
        {
            get 
            {
                return _enabled;
            }
        }   

        public bool IsBuff 
        { 
            get 
            { 
                return _isBuff; 
            } 
        }

        public bool HasEffectWore 
        {
            get 
            {
                return _effectWore;
            }
        }
    }
}
