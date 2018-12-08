using System;
using System.Collections.Generic;
using System.Linq;
using EasyFarm.Classes;
using EasyFarm.States;
using EasyFarm.Tests.TestTypes.Mocks;
using MemoryAPI;
using MemoryAPI.Navigation;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class TargetPriorityTests
    {
        [Fact]
        public void WillOrderTargetsByPriority()
        {
            // Fixture setup
            IUnit[] units = InitializeUnits(numberUnits: 3);
            units[0] = FindPartyClaimedMob("PartyClaim");
            units[1] = FindAggroedMob("Aggroed");
            units[2] = FindLongDistanceMob("ShortestDistance");
            units = ShuffleUnits(units);

            // Excercise system
            IUnit[] result = TargetPriority.Prioritize(units).ToArray();
            // Verify outcome
            Assert.Equal("PartyClaim", result[0].Name);
            Assert.Equal("Aggroed", result[1].Name);
            Assert.Equal("ShortestDistance", result[2].Name);
            // Teardown	
        }

        private static IUnit[] ShuffleUnits(IUnit[] units)
        {
            return units.OrderBy(x => new Random().Next(0, 3)).ToArray();
        }

        private static IUnit[] InitializeUnits(int numberUnits)
        {
            return Enumerable.Range(0, 3).Select(x => new MockUnit()).Cast<IUnit>().ToArray();
        }

        private MockUnit FindPartyClaimedMob(string name)
        {
            var mob = ValidMob();
            mob.Name = name;
            SetPartyClaimed(mob);
            return mob;
        }

        private MockUnit FindAggroedMob(string name)
        {
            var mob = ValidMob();
            mob.Name = name;
            SetAggroedMob(mob);
            return mob;
        }

        private MockUnit FindLongDistanceMob(string name)
        {
            var mob = ValidMob();
            mob.Name = name;
            SetLongDistance();
            return mob;
        }

        private MockUnit SetLongDistance()
        {
            var mob = ValidMob();
            mob.Distance = 1;
            return mob;
        }

        private static void SetAggroedMob(MockUnit mob)
        {
            mob.Status = Status.Fighting;
            mob.IsClaimed = false;
        }

        private void SetPartyClaimed(MockUnit mob)
        {
            mob.IsClaimed = true;
        }

        private static MockUnit ValidMob()
        {
            return new MockUnit()
            {
                Name = "Mandragora",
                Status = Status.Standing,
                Distance = 5,
                IsActive = true,
                IsClaimed = false,
                IsRendered = true,
                NpcType = NpcType.Mob,
                PosX = 1,
                PosY = 1,
                PosZ = 1,
                Position = new Position
                {
                    X = 1,
                    H = 1,
                    Y = 1,
                    Z = 1
                }
            };
        }
    }
}
