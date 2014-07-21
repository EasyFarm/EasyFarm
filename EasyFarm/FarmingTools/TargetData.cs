
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
*////////////////////////////////////////////////////////////////////


using EasyFarm.GameData;
using FFACETools;

namespace ZeroLimits.FarmingTool
{
    /// <summary>
    /// A class about the players target. 
    /// </summary>
    public class TargetData
    {
        private FFACE _fface;

        private Unit _targetUnit = Unit.CreateUnit(0);

        public TargetData(FFACE fface)
        {
            this._fface = fface;
        }

        /// <summary>
        /// Who we are trying to kill currently
        /// </summary>
        public Unit TargetUnit
        {
            get { return _targetUnit; }
            set { this._targetUnit = value; }
        }

        /// <summary>
        /// Can we pull the target unit?
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsPullable
        {
            get
            {
                return IsTarget && 
                    !IsFighting && 
                    FarmingTools.GetInstance(_fface).PlayerActions.HasStartMoves;
            }
        }

        /// <summary>
        /// Is our current target our target unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsTarget
        {
            get
            {
                return _fface.Target.ID == TargetUnit.ID;
            }
        }

        /// <summary>
        /// Is the targets status == fighting
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsFighting
        {
            get { return TargetUnit.Status == Status.Fighting; }
        }

        /// <summary>
        /// Returns true if the units health is 0%
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsDead
        {
            get { return TargetUnit.Status.Equals(Status.Dead1) || TargetUnit.Status.Equals(Status.Dead2); }
        }

        /// <summary>
        /// Can we battle the target unit?
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsValid
        {
            get
            {
                return FarmingTools.GetInstance(_fface).UnitService.IsValid(TargetUnit);
            }
        }

        public FFACE.Position Position
        {
            get { return TargetUnit.Position; }
        }
    }
}
