using EasyFarm.Engine;
using EasyFarm.UnitTools;
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
        public TargetData(ref GameEngine Engine)
        {
            this.Engine = Engine;
        }

        private Unit m_targetUnit = null;

        /// <summary>
        /// Who we are trying to kill currently
        /// </summary>
        public Unit TargetUnit
        {
            get
            {
                return m_targetUnit == null ||
                !Engine.Units.IsValid(m_targetUnit) ?
                m_targetUnit = Engine.Units.Target : m_targetUnit;
            }

            set { this.TargetUnit = value; }
        }

        /// <summary>
        /// Can we pull the target unit?
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsUnitPullable
        {
            get
            {
                return IsTargetUnit && !IsUnitFighting && Engine.Combat.HasStartMoves;
            }
        }

        /// <summary>
        /// Is our current target our target unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsTargetUnit
        {
            get
            {
                return Engine.FFInstance.Instance.Target.ID == TargetUnit.ID;
            }
        }

        /// <summary>
        /// Is the targets status == fighting
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsUnitFighting
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
        public bool IsUnitBattleReady
        {
            get
            {
                return Engine.Units.IsValid(TargetUnit);
            }
        }

        public FFACE.Position Position
        {
            get { return TargetUnit.Position; }
        }

        public GameEngine Engine { get; set; }
    }
}
