#define Debug

using System;
using System.Collections.Generic;
using System.Linq;
using EasyFarm.Engine;
using FFACETools;

namespace EasyFarm.UnitTools
{
    public class Units
    {
        #region Members

        private static Unit[] UnitArray;
        private static GameState GameState;
        private const short MOBARRAY_MAX = 768;

        #endregion

        #region Properties

        public bool HasAggro
        {
            get
            {
                foreach (var Monster in ValidMobs)
                {
                    if (Monster.HasAggroed)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool HasClaim
        {
            get
            {
                foreach(var Monster in ValidMobs)
                {
                    if (Monster.IsClaimed)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public Unit[] ValidMobs
        {
            get
            {                
                return UnitArray
                    .Where(x => !x.Name.Equals(String.Empty))
                    .Where(x => IsValid(x))
                    .ToArray();
            }
        }

        public Unit GetTarget()
        {
            // Create a blank target
            var MainTarget = Unit.CreateUnit(0);
            
            // Create a copy of the valid mobs
            Unit[] PotentialTargets = ValidMobs;

            try
            {
                if (GameState.Config.BattlePartyClaimed && PotentialTargets.Where(mob => mob.PartyClaim).Count() > 0)
                {
                    MainTarget = PotentialTargets.First(mob => mob.PartyClaim);
                }
                else if (PotentialTargets.Where(mob => mob.MyClaim).Count() > 0)
                {
                    MainTarget = PotentialTargets.First(mob => mob.MyClaim);
                }
                else if (GameState.Config.BattleAggro && PotentialTargets.Where(mob => mob.HasAggroed).Count() > 0)
                {
                    MainTarget = PotentialTargets.First(mob => mob.HasAggroed);
                }
                else if (GameState.Config.BattleUnclaimed && PotentialTargets.Where(mob => !mob.IsClaimed).Count() > 0)
                {
                    MainTarget = PotentialTargets.Where(mob => !mob.IsClaimed).First();
                }
            }
            catch (InvalidOperationException)
            {
                // Do Nothing, let bot retry
            }

            return MainTarget;
        }

        #endregion

        #region Constructors

        public Units(ref GameState State)
        {
            GameState = State;
            Unit.Session = State.FFInstance.Instance;
            UnitArray = new Unit[MOBARRAY_MAX];

            // Create units
            for (int id = 0; id < MOBARRAY_MAX; id++)
            {
                UnitArray[id] = Unit.CreateUnit(id);
            }
        }

        public bool IsValid(Unit unit)
        {
            bool ValidMob =
                ((unit.IsActive) && (unit.Distance < 17) && (unit.YDifference < 5) && (unit.NPCBit != 0) && (!unit.IsDead) && (unit.NPCType == (NPCType)16))
                &&
                (((GameState.Config.TargetsList.Contains(unit.Name) && !unit.IsClaimed) || (GameState.Config.TargetsList.Count == 0 && !GameState.Config.IgnoredList.Contains(unit.Name)))
                ||
                ((unit.HasAggroed) || (unit.MyClaim) || (unit.PartyClaim)));

            return ValidMob;
        }

        #endregion
    }
}
