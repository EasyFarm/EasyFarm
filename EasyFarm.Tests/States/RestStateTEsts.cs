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
using EasyFarm.Tests.TestTypes.Mocks;
using MemoryAPI;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class RestStateTests
    {
        private readonly TestContext context = new TestContext();
        private readonly RestState sut = new RestState();

        [Theory]
        [InlineData(50, 50, true)]
        [InlineData(50, 51, true)]
        [InlineData(50, 49, false)]
        public void RestWhenMpLow(int currentMPP, int lowMPP, bool expected)
        {
            // Setup fixture
            context.Player.MppCurrent = currentMPP;
            context.Config.IsMagicEnabled = true;
            context.Config.LowMagic = lowMPP;
            // Exercise system
            bool result = sut.Check(context);
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Theory]
        [InlineData(50, 50, true)]
        [InlineData(50, 51, true)]
        [InlineData(50, 49, false)]
        public void RestWhenHpLow(int currentHPP, int lowHPP, bool expected)
        {
            // Setup fixture
            context.Player.HppCurrent = currentHPP;
            context.Config.IsHealthEnabled = true;
            context.Config.LowHealth = lowHPP;
            // Exercise system
            bool result = sut.Check(context);
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void DontRestWithAggro()
        {
            // Fixture setup
            context.Player.HasAggro = true;
            // Exercise system
            bool result = sut.Check(context);
            // Verify outcome
            Assert.False(result);
            // Teardown	
        }

        [Fact]
        public void SitDownToRest()
        {
            // Setup fixture
            context.MockAPI.Player.Status = Status.Standing;
            // Exercise system
            sut.Run(context);
            // Verify outcome
            Assert.Equal(Status.Healing, context.MockAPI.Player.Status);
            // Teardown
        }


        [Fact]
        public void StandUpAfterResting()
        {
            // Setup fixture
            context.MockAPI.Player.Status = Status.Healing;
            // Exercise system
            sut.Exit(context);
            // Verify outcome
            Assert.Equal(Status.Standing, context.MockAPI.Player.Status);
            // Teardown
        }

        [Fact]
        public void DontRestWithRestBlockingStatusEffect()
        {
            // Setup fixture
            context.SetPlayerInjured();
            context.MockAPI.Player.StatusEffects = new StatusEffect[] {StatusEffect.Bio};
            // Exercise system
            bool result = sut.Check(context);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void DontRestWhenFighting()
        {
            // Setup fixture
            context.SetPlayerInjured();
            context.Player.Status = Status.Fighting;
            // Exercise system
            bool result = sut.Check(context);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }
    }
}
