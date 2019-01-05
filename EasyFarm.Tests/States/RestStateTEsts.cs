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
using EasyFarm.States;
using EasyFarm.Tests.Context;
using MemoryAPI;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class RestStateTests
    {
        public class CheckTests
        {
            [Theory]
            [InlineData(50, 50, true)]
            [InlineData(50, 51, true)]
            [InlineData(50, 49, false)]
            public void ShouldHealWithLowMP(int currentMPP, int lowMPP, bool expected)
            {
                // Setup fixture
                var context = new TestContext();
                context.Player.MppCurrent = currentMPP;
                context.Config.IsMagicEnabled = true;
                context.Config.LowMagic = lowMPP;
                var sut = CreateSut();
                // Exercise system
                var result = sut.Check(context);
                // Verify outcome
                Assert.Equal(expected, result);
                // Teardown
            }

            [Theory]
            [InlineData(50, 50, true)]
            [InlineData(50, 51, true)]
            [InlineData(50, 49, false)]
            public void ShouldHealWithLowHP(int currentHPP, int lowHPP, bool expected)
            {
                // Setup fixture
                var context = new TestContext();
                context.Player.HppCurrent = currentHPP;
                context.Config.IsHealthEnabled = true;
                context.Config.LowHealth = lowHPP;
                RestState sut = CreateSut();
                // Exercise system
                var result = sut.Check(context);
                // Verify outcome
                Assert.Equal(expected, result);
                // Teardown
            }

            [Fact]
            public void HasAggro_NoResting()
            {
                // Fixture setup
                var context = new TestContext();
                context.Player.HasAggro = true;
                var sut = CreateSut();
                // Exercise system
                var result = sut.Check(context);
                // Verify outcome
                Assert.False(result);
                // Teardown	
            }

            private RestState CreateSut()
            {
                return new RestState();
            }
        }

        public class RunTests
        {
            [Fact]
            public void PlayerSwitchesFromStandingToHealing()
            {
                // Setup fixture
                var context = new TestContext();
                context.MockAPI.Player.Status = Status.Standing;
                var sut = CreateSut();
                // Exercise system
                sut.Run(context);
                // Verify outcome
                Assert.Equal(Status.Healing, context.MockAPI.Player.Status);
                // Teardown
            }

            private RestState CreateSut()
            {
                return new RestState();
            }
        }

        public class ExitTests
        {
            [Fact]
            public void PlayerSwitchesFromHealingToStanding()
            {
                // Setup fixture
                var context = new TestContext();
                context.MockAPI.Player.Status = Status.Healing;
                var sut = CreateSut();
                // Exercise system
                sut.Exit(context);
                // Verify outcome
                Assert.Equal(Status.Standing, context.MockAPI.Player.Status);
                // Teardown
            }

            private RestState CreateSut()
            {
                return new RestState();
            }
        }
    }
}
