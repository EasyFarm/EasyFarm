#define Debug

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFACETools;

namespace EasyFarm.UnitTools
{
    [Serializable]
    public class Units
    {
        #region Properties
        public List<string> TargetNames = new List<string>();
        public List<string> IgnoredMobs = new List<string>();

        [NonSerialized]
        private FFACE Session;
        public bool HasAggro
        {
            get
            {
                var IsAggroed = false;

                foreach (var mob in GetMobs(MobArray))
                {
                    IsAggroed |= mob.HasAggroed;
                }

                return IsAggroed;
            }
        }
        public bool HasClaim
        {
            get
            {
                var MyClaim = false;
                foreach (var mob in GetMobs(MobArray))
                {
                    MyClaim |= mob.MyClaim;
                }
                return MyClaim;
            }
        }
        [NonSerialized]
        public List<Unit> MobArray = new List<Unit>();
        public List<Unit> Mobs { get { return GetMobs(MobArray); } }

        #endregion

        #region Constructors
        private Units()
        {

        }

        public Units(FFACE session)
        {
            Session = session;
            Unit.Session = session;
            MobArray = CreateUnits();
        }
        #endregion

        #region Methods
        private List<Unit> CreateUnits()
        {
            var Units = new List<Unit>();

            for (int id = 0; id < 768; id++)
                Units.Add(Unit.CreateUnit(id));

            return Units;
        }

        private List<Unit> GetMobs(List<Unit> mobArray)
        {            
            #if Debug            
            var MobsWithNamesQuery = from m in mobArray
                                where m.Name != ""
                                select m;
            var MobsWithNamesList = MobsWithNamesQuery.ToList();  
            #endif

            var Mobs = from m in mobArray
                       where m.Name != ""
                       where IsValid(m)
                       select m;
            var MobList = Mobs.ToList();
            return MobList;
        }

        public bool IsValid(Unit unit)
        {
            if (Session == null)
                return false;

            double YDifference = Math.Abs(unit.PosY - Session.Player.PosY);

            bool ValidMob =
                ((unit.IsActive) && (unit.Distance < 17) && (YDifference < 5) && (unit.NPCBit != 0) &&(!unit.IsDead) && (unit.NPCType == (NPCType)16))
                &&
                (((TargetNames.Contains(unit.Name) && !unit.IsClaimed) || (TargetNames.Count == 0 && !IgnoredMobs.Contains(unit.Name)))
                ||
                ((unit.HasAggroed) || (unit.MyClaim) || (unit.PartyClaim)));

            return ValidMob;
        }
        #endregion
    }
}
