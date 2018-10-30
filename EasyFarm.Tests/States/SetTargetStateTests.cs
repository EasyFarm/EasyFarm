using EasyFarm.States;
using EasyFarm.Tests.TestTypes.Mocks;
using MemoryAPI;
using MemoryAPI.Navigation;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class SetTargetStateTests
    {
        private static readonly MockGameAPI MockGameAPI = new MockGameAPI();

        [Fact]
        public void SetTargetSetsCurrentTargetWithValidMob()
        {
            // Fixture setup
            MockGameAPI.Mock.NPC.Entities[0] = FindUnclaimedMob();
            var sut = CreateSut();
            sut.Config = SetupConfigToKillUnclaimedMobs();
            // Excercise system
            sut.Check();
            // Verify outcome
            Assert.NotNull(sut.Target);
            // Teardown	
        }

        private static MockConfig SetupConfigToKillUnclaimedMobs()
        {
            return new MockConfig
            {
                DetectionDistance = 10,
                UnclaimedFilter = true,
                HeightThreshold = 10,
            };
        }

        private static MockNPC FindUnclaimedMob()
        {
            return new MockNPC()
            {
                Name = "Mandragora",
                Status = Status.Standing,
                ClaimID = 0,
                Distance = 5,
                HealthPercent = 100,
                IsActive = true,
                IsClaimed = false,
                IsRendered = true,
                NPCType = NpcType.Mob,
                PetID = 0,
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

        private static SetTargetState CreateSut()
        {
            return new SetTargetState(new StateMemory(MockGameAPI));
        }
    }
}
