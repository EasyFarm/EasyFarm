using EasyFarm.Classes;
using EasyFarm.States;
using EasyFarm.Tests.Context;
using EasyFarm.Tests.TestTypes;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class WeaponSkillStateTests
    {
        private readonly WeaponskillState _sut = new WeaponskillState();
        private readonly TestContext _context = new TestContext();

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
            _context.SetTargetInvalid();
            Assert.False(_sut.Check(_context));
            // Teardown
        }

        [Fact]
        public void Check_RunWhenPlayerIsHealthy_TargetsIsValid_And_PlayerIsFighting()
        {
            // Setup fixture
            _context.SetPlayerHealthy();
            _context.SetTargetValid();
            _context.SetPlayerFighting();
            // Exercise system
            // Verify outcome
            Assert.True(_sut.Check(_context));
            // Teardown
        }

        [Fact]
        public void Run_UsesWeaponSkill()
        {
            // Setup fixture
            BattleAbility battleAbility = AbstractTestBase.FindAbility();
            battleAbility.Command = "/test";
            _context.Config.BattleLists["Weaponskill"].Actions.Add(battleAbility);
            _context.SetTargetValid();
            _context.SetPlayerFighting();
            // Exercise system
            // Verify outcome
            _sut.Run(_context);
            Assert.Equal("/test", _context.Memory.Executor.LastCommand);
            // Teardown
        }

        [Fact(Skip = "Race")]
        public void Run_WithBadWeaponSkill_DoesNotUseIt()
        {
            // Setup fixture
            BattleAbility battleAbility = AbstractTestBase.FindAbility();
            battleAbility.Command = "/test";
            battleAbility.IsEnabled = false;
            _context.Config.BattleLists["Weaponskill"].Actions.Add(battleAbility);
            _context.SetTargetValid();
            _context.SetPlayerFighting();
            // Exercise system
            _sut.Run(_context);
            // Verify outcome
            Assert.NotEqual("/test", _context.Memory.Executor.LastCommand);
            // Teardown
        }
    }
}
