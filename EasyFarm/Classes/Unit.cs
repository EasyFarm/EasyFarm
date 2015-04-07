
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

namespace EasyFarm.Classes
{
    public class Unit
    {
        #region Members
        /// <summary>
        /// Holds all the game's data. 
        /// </summary>
        private FFACE _fface;

        /// <summary>
        /// Holds the data about units. 
        /// </summary>
        private FFACE.NPCTools _npc;
        #endregion

        #region Constructors
        public Unit(FFACE fface, int id)
        {
            // Set this unit's session data. 
            _fface = fface;

            // Set the internal id. 
            ID = id;

            // Set the NPC information.
            _npc = _fface.NPC;
        }
        #endregion

        #region Player Data
        /// <summary>
        /// The unit's id. 
        /// </summary>
        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// The unit's claim id; zero for unclaimed. 
        /// </summary>
        public int ClaimedID
        {
            get { return _npc.ClaimedID(ID); }
        }

        /// <summary>
        /// The unit's distace from the player. 
        /// </summary>
        public double Distance
        {
            get { return _npc.Distance(ID); }
        }

        /// <summary>
        /// The unit's position. 
        /// </summary>
        public FFACE.Position Position
        {
            get { return _npc.GetPosition(ID); }
        }

        /// <summary>
        /// The unit's health as a percent. 
        /// </summary>
        public short HPPCurrent
        {
            get { return _npc.HPPCurrent(ID); }
        }

        /// <summary>
        /// Whether this unit is active. 
        /// </summary>
        public bool IsActive
        {
            get { return _npc.IsActive(ID); }
        }

        /// <summary>
        /// Whether this unit is claimed by some player. 
        /// </summary>
        public bool IsClaimed
        {
            get { return _npc.IsClaimed(ID); }
        }

        /// <summary>
        /// Whether this unit is visible to the player. 
        /// </summary>
        public bool IsRendered
        {
            get { return _npc.IsRendered(ID); }
        }

        /// <summary>
        /// The unit's name. 
        /// </summary>
        public string Name
        {
            get { return _npc.Name(ID); }
        }

        /// <summary>
        /// The unit's npc bit
        /// </summary>
        public byte NPCBit
        {
            get { return _npc.NPCBit(ID); }
        }

        /// <summary>
        /// The unit's npc type
        /// </summary>
        public NPCType NPCType
        {
            get { return _npc.NPCType(ID); }
        }

        /// <summary>
        /// The unit's pet's id. 
        /// </summary>
        public int PetID
        {
            get { return _npc.PetID(ID); }
        }

        /// <summary>
        /// The unit's heading. 
        /// </summary>
        public float PosH
        {
            get { return _npc.PosH(ID); }
        }

        /// <summary>
        /// The unit's x coordinate. 
        /// </summary>
        public float PosX
        {
            get { return _npc.PosX(ID); }
        }

        /// <summary>
        /// The unit's y coordinate. 
        /// </summary>
        public float PosY
        {
            get { return _npc.PosY(ID); }
        }

        /// <summary>
        /// The unit's z coordinate.  
        /// </summary>
        public float PosZ
        {
            get { return _npc.PosZ(ID); }
        }

        /// <summary>
        /// The unit's status. 
        /// </summary>
        public Status Status
        {
            get { return _npc.Status(ID); }
        }

        /// <summary>
        /// The unit's current tp. 
        /// </summary>
        public short TPCurrent
        {
            get { return _npc.TPCurrent(ID); }
        }

        public bool MyClaim
        {
            // Using FFACE.PartyMember[0].ServerID until FFACE.Player.PlayerServerID is fixed. 
            get { return ClaimedID == _fface.PartyMember[0].ServerID; }
        }

        /// <summary>
        /// If the unit has aggroed our player. 
        /// </summary>
        public bool HasAggroed
        {
            get
            {
                return (!IsClaimed || MyClaim) && Status == FFACETools.Status.Fighting;
            }
        }

        /// <summary>
        /// If the unit is dead. 
        /// </summary>
        public bool IsDead
        {
            get { return Status == FFACETools.Status.Dead1 || Status == FFACETools.Status.Dead2 || HPPCurrent <= 0; }
        }

        /// <summary>
        /// If a party or alliance member has claim on the unit. 
        /// </summary>
        public bool PartyClaim
        {
            get
            {
                for (byte i = 0; i < _fface.PartyMember.Count; i++)
                {
                    if (_fface.PartyMember[i].ServerID != 0 && ClaimedID == _fface.PartyMember[i].ServerID)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// The vertical distance between this unit and our player. 
        /// </summary>
        public double YDifference
        {
            get { return Math.Abs(PosY - _fface.Player.PosY); }
        }

        #endregion

        #region Methods

        // Make it default to printing units name
        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}