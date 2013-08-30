using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFACETools;

namespace EasyFarm.UnitTools
{
    public class Unit
    {
        #region Members
        public static FFACE Session;
        static FFACE.NPCTools NPCTools;
        static Unit m = null; 
        #endregion

        #region Constructors
        private Unit()
        {

        }

        Unit(int id = 0)
        {
            if (NPCTools == null)
                NPCTools = Session.NPC;

            ID = id;
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

        // Details about a specific unit
        // Retrieves detail from FFACE Library
        // Uses ID to get details from NPCTools
        #region Player Data

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
            get { return IsClaimed && ClaimedID == Session.Player.PlayerServerID; }
        }
        public bool HasAggroed
        {
            get { return !IsClaimed && Status == FFACETools.Status.Fighting; }
        }
        public bool IsDead
        {
            get { return Status == FFACETools.Status.Dead1 || Status == FFACETools.Status.Dead2 || HPPCurrent <= 0; }
        }
        public bool PartyClaim 
        {
            get 
            {
                foreach ( var player in Session.PartyMember.Values )
                    if ( this.ClaimedID == player.ServerID && player.ServerID != 0)
                        return true;
                return false;
            }
        }

        #endregion

        // Make it default to printing units name
        public override string ToString()
        {
            return this.Name.ToString();
        }
    }
}