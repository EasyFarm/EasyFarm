// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
//  
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using EasyFarm.Infrastructure;
using EasyFarm.States;
using EasyFarm.Tests.Context;
using EasyFarm.Tests.TestTypes;
using EliteMMO.API;
using MemoryAPI;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class DeadStateTests : AbstractTestBase
    {
        private static readonly TestContext context = new TestContext();
        private static readonly DeadState sut = new DeadState();

        [Theory]
        [InlineData(Status.Dead1, true)]
        [InlineData(Status.Dead2, true)]
        [InlineData(Status.Standing, false)]
        [InlineData(Status.Fighting, false)]
        public void CheckTrueWhenPlayersDead(Status status, bool expected)
        {
            // Setup fixture
            context.Player.Status = status;1
            // Exercise system
            bool result = sut.Check(context);
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void RunResetIsCalledToStopPlayerFromRunning()
        {
            // Setup fixture
            context.MockAPI.Navigator.IsRunning = true;
            // Exercise system
            sut.Run(context);
            // Verify outcome
            Assert.False(context.MockAPI.Navigator.IsRunning);
            // Teardown
        }

        [Fact]
        public void RunDoesNotHomePointIfConfigNotSet()
        {
            // Setup fixture
            // Exercise system
            sut.Run(context);
            // Verify outcome
            Assert.Empty(context.MockAPI.Windower.KeyPresses);
            // Teardown
        }

        [Fact]
        public void RunSendsPauseCommand()
        {
            // Setup fixture
            // Exercise system
            sut.Run(context);
            // Verify outcome
            Assert.Contains(typeof(Events.PauseEvent), Events);
            // Teardown
        }

        [Fact]
        public void RunAttemptsToHomepointAfterDeath()
        {
            // Setup fixture
            context.Config.HomePointOnDeath = true;
            List<Keys> expected = new List<Keys> { Keys.NUMPADENTER, Keys.LEFT, Keys.NUMPADENTER };
            // Exercise system
            sut.Run(context);
            // Verify outcome
            Assert.Equal(expected, context.MockAPI.Windower.KeyPresses);
            // Teardown
        }
    }
}
