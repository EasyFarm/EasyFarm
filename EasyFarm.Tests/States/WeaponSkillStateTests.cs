using EasyFarm.States;
using EasyFarm.Tests.Context;
using MemoryAPI;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class WeaponSkillStateTests
    {
        private WeaponskillState _sut = new WeaponskillState();
        private TestContext _context = new TestContext();

        [Fact]
        public void Check_DoNotRunWhenPlayerInjured()
        {
            _context.SetPlayerInjured();
            Assert.False(_sut.Check(_context));
        }

        [Fact]
        public void Check_DoNotRunWithInvalidTarget()
        {
            _context.SetPlayerHealthy();
            _context.SetInvalidTarget();
            Assert.False(_sut.Check(_context));
            // Teardown
        }
    }
}
