
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

using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using ZeroLimits.XITool.Interfaces;
using FFACETools;

namespace ZeroLimits.XITool.Classes
{
    public class Unit : IUnit
    {
        #region Members
        public static FFACE Session { get; set; }
        private static FFACE.NPCTools NPCTools;
        private static Unit m = null;
       
        #endregion

        #region Constructors
        protected Unit(int id = 0)
        {
            // Set the internal id. 
            this.ID = id;

            // Our npctools object. 
            if (NPCTools == null && Session != null)
            {
                NPCTools = Session.NPC;
            }            
        }

        public static Unit CreateUnit(int id)
        {
            if (m == null)
            {
                return m = new Unit(id);
            }
            else
            {
                m.ID = id;
                return m.MemberwiseClone() as Unit;
            }
        }
        #endregion        

        #region Player Data
        // Details about a specific unit
        // Retrieves detail from FFACE Library
        // Uses ID to get details from NPCTools

        public int ID
        {
            get;
            set;
        }

        public int ClaimedID
        {
            get { return NPCTools.ClaimedID(ID); }
        }

        public double Distance
        {
            get { return NPCTools.Distance(ID); }
        }

        public FFACE.Position Position
        {
            get { return NPCTools.GetPosition(ID); }
        }

        public short HPPCurrent
        {
            get { return NPCTools.HPPCurrent(ID); }
        }

        public bool IsActive
        {
            get { return NPCTools.IsActive(ID); }
        }

        public bool IsClaimed
        {
            get { return NPCTools.IsClaimed(ID); }
        }

        public bool IsRendered
        {
            get { return NPCTools.IsRendered(ID); }
        }

        public string Name
        {
            get { return NPCTools.Name(ID); }
        }

        public byte NPCBit
        {
            get { return NPCTools.NPCBit(ID); }
        }

        public NPCType NPCType
        {
            get { return NPCTools.NPCType(ID); }
        }

        public int PetID
        {
            get { return NPCTools.PetID(ID); }
        }

        public float PosH
        {
            get { return NPCTools.PosH(ID); }
        }

        public float PosX
        {
            get { return NPCTools.PosX(ID); }
        }

        public float PosY
        {
            get { return NPCTools.PosY(ID); }
        }

        public float PosZ
        {
            get { return NPCTools.PosZ(ID); }
        }

        public byte[] RawData
        {
            get { return NPCTools.GetRawNPCData(ID, ID, ID); }
        }

        public Status Status
        {
            get { return NPCTools.Status(ID); }
        }

        public short TPCurrent
        {
            get { return NPCTools.TPCurrent(ID); }
        }

        public bool MyClaim
        {
            // Using FFACE.PartyMember[0].ServerID until FFACE.Player.PlayerServerID is fixed. 
            get { return ClaimedID == Session.PartyMember[0].ServerID; }
        }

        public bool HasAggroed
        {
            get
            {
                return (!IsClaimed || MyClaim) && Status == FFACETools.Status.Fighting;
            }
        }

        public bool IsDead
        {
            get { return Status == FFACETools.Status.Dead1 || Status == FFACETools.Status.Dead2 || HPPCurrent <= 0; }
        }

        public bool PartyClaim
        {
            get
            {
                for (byte i = 0; i < Session.PartyMember.Count; i++)
                {
                    if (Session.PartyMember[i].ServerID != 0 && ClaimedID == Session.PartyMember[i].ServerID)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public double YDifference
        {
            get { return Math.Abs(PosY - Session.Player.PosY); }
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