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

using System.Linq;
using EasyFarm.States;
using EasyFarm.Tests.Context;
using EasyFarm.ViewModels;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class SetTargetStateTests
    {
        private static readonly TestContext context = new TestContext();
        private static readonly SetTargetState sut = new SetTargetState();

        [Fact]
        public void WillTargetValidMob()
        {
            // Fixture setup
            context.Units[0].IsValid = true;
            // Exercise system
            sut.Check(context);
            // Verify outcome
            Assert.NotNull(context.Target);
            // Teardown	
        }

        [Fact]
        public void OnlyUpdateUserAboutTargetSwitchWithValidTarget()
        {
            // Fixture setup
            LogViewModel viewModel = new LogViewModel();
            context.Units[0].IsValid = true;
            // Exercise system
            sut.Check(context);
            // Verify outcome
            Assert.True(viewModel.LoggedItems.Any());
            // Teardown	
        }
    }
}