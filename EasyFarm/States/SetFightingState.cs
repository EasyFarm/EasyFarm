// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
using System.Linq;
using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.States
{
    public class SetFightingState : CombatState
    {
        public SetFightingState(IMemoryAPI fface) : base(fface) { }

        public override bool Check()
        {
            if (UnitFilters.MobFilter(fface, Target))
            {
                // No moves in pull list, set FightStarted to true to let
                // other components who depend on it trigger. 
                if (!Config.Instance.BattleLists["Pull"].Actions.Any(x => x.IsEnabled))
                {
                    return IsFighting = true;
                }
                else
                {
                    return IsFighting = Target.Status.Equals(Status.Fighting);
                }
            }

            return IsFighting = false;
        }
    }
}
