using EasyFarm.Classes;
using EasyFarm.Tests.TestTypes.Mocks;
using EasyFarm.UserSettings;
using MemoryAPI;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class PlayerTests
    {
        [Fact]
        public void AttackOffWhenDisengageCalledWhenFighting()
        {
            // Fixture setup
            var mockApi = new MockGameAPI();
            mockApi.Mock.Player.Status = Status.Fighting;
            Player.Disengage(mockApi);
            // Excercise system
            Assert.Equal(Constants.AttackOff, mockApi.Mock.Windower.LastCommand);
            // Verify outcome
            // Teardown	
        }
    }
}
