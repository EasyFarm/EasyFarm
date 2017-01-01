using System;
using Xunit;
using EasyFarm.Classes;
using MemoryAPI.Tests;

namespace EasyFarm.Tests.Classes
{
    public class ExecutorTests
    {
        private static readonly FakeMemoryAPI Memory = new FakeMemoryAPI();
        private static readonly FakePlayer Player = new FakePlayer();

        public class UseBuffingAction
        {
            [Fact]
            public void UseBuffingActionWithNullAbilityThrows()
            {
                var sut = CreateSut();
                var result = Record.Exception(() => sut.UseBuffingAction(null));
                Assert.IsType<ArgumentNullException>(result);
            }
        }

        public class UseBuffingActions
        {
            [Fact]
            public void UseBuffingActionsWithNullActionListThrows()
            {
                var sut = CreateSut();
                var result = Record.Exception(() => sut.UseBuffingActions(null));
                Assert.IsType<ArgumentNullException>(result);
            }
        }        

        private static Executor CreateSut()
        {
            Memory.Player = Player;
            var sut = new Executor(Memory);
            return sut;
        }
    }
}
