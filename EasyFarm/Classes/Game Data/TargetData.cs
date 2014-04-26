
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

using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    /// <summary>
    /// A class about the players target. 
    /// </summary>
    public class TargetData
    {
        public TargetData(ref GameEngine gameEngine)
        {
            this._engine = gameEngine;
        }

        private Unit m_targetUnit = null;       
        private GameEngine _engine;

        /// <summary>
        /// Who we are trying to kill currently
        /// </summary>
        public Unit TargetUnit
        {
            get
            {
                var Units = _engine.Units;
                return m_targetUnit == null ||
                !Units.IsValid(m_targetUnit) ?
                m_targetUnit = Units.GetTarget() : m_targetUnit;
            }

            set { this.TargetUnit = value; }
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
                var PlayerActions = _engine.PlayerActions;
                return IsTarget && !IsFighting && PlayerActions.HasStartMoves;
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
                var TargetTools = _engine.Session.Instance.Target;
                return TargetTools.ID == TargetUnit.ID;
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
            get { return TargetUnit.HPPCurrent <= 0; }
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
                var Units = _engine.Units;
                return Units.IsValid(TargetUnit);
            }
        }

        public FFACE.Position Position
        {
            get { return TargetUnit.Position; }
        }
    }
}
