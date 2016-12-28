using System;
using Xunit;
using Moq;
using EasyFarm.Classes;
using EasyFarm.Parsing;
using MemoryAPI;
using MemoryAPI.Tests;

namespace EasyFarm.Tests.Classes
{
    public class ExecutorTests
    {
        [Fact]
        public void UseBuffingActionWithNullAbilityThrows()
        {
            var memory = new FakeMemoryAPI();
            var player = new FakePlayer();
            memory.Player = player;

            var sut = new Executor(memory);
            var result = Record.Exception(() => sut.UseBuffingAction(null));

            Assert.IsType<ArgumentNullException>(result);
        }
    }
}
