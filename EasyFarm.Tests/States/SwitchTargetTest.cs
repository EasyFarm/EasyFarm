using EasyFarm.States;
using EasyFarm.Tests.TestTypes.Mocks;
using MemoryAPI;
using MemoryAPI.Navigation;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class SwitchTargetTest
    {
        [Fact]
        public void UserCanSwitchTargetToAnotherValidMob()
        {
            // Fixture setup
            var api = new MockGameAPI();
            var sut = new SetTargetState(new StateMemory(api));
            sut.Config = new MockConfig()
            {
                UnclaimedFilter = true,
                DetectionDistance = 10,
                HeightThreshold = 10,
            };
            var units = new MockUnitService();
            sut.Memory.UnitService = units;

            // Current Target
            var currentTarget = ValidMob();
            currentTarget.Id = 10;
            units.MobArray.Add(currentTarget);
            sut.Target = currentTarget;

            // New Target
            var newTarget = ValidMob();
            newTarget.Id = 20;
            api.Mock.Target.ID = 20;                     
            units.MobArray.Add(newTarget);            

            // Excercise system
            sut.Check();
            // Verify outcome
            Assert.Equal(newTarget, sut.Target);
            // Teardown	
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