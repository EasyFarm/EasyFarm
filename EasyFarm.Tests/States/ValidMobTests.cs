using EasyFarm.States;
using EasyFarm.Tests.Context;
using EasyFarm.Tests.TestTypes.Mocks;
using MemoryAPI;
using MemoryAPI.Navigation;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class ValidMobTests
    {
        [Fact]
        public void WillTargetValidMob()
        {
            // Fixture setup
            var api = new MockGameAPI();
            var sut = new SetTargetState();
            var context = new TestContext();
            //var context = new StateMemory(api);
            context.Config = KillUnclaimedMobsConfig();
            context.Memory.UnitService = new MockUnitService()
            {
                MobArray = {FindUnclaimedMob()}
            };
            // Excercise system
            sut.Check(context);
            // Verify outcome
            Assert.NotNull(context.Target);
            // Teardown	
        }

        private static MockConfig KillUnclaimedMobsConfig()
        {
            return new MockConfig
            {
                DetectionDistance = 10,
                UnclaimedFilter = true,
                HeightThreshold = 10,
            };
        }

        private static MockUnit FindUnclaimedMob()
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