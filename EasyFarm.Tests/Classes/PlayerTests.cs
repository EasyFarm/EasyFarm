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
using EasyFarm.Classes;
using EasyFarm.Tests.TestTypes.Mocks;
using EasyFarm.UserSettings;
using MemoryAPI;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class PlayerTests
    {
        private readonly MockGameAPI _mockApi = new MockGameAPI();

        [Fact]
        public void DisengageWhenFightingWillStopAttacking()
        {
            // Fixture setup            
            _mockApi.Mock.Player.Status = Status.Fighting;
            // Excercise system
            Player.Disengage(_mockApi);
            // Verify outcome
            Assert.Equal(Constants.AttackOff, _mockApi.Mock.Windower.LastCommand);
            // Teardown	
        }

        [Fact]
        public void DisengageWhenNotFightDoesNothing()
        {
            // Fixture setup            
            _mockApi.Mock.Player.Status = Status.Standing;
            // Excercise system
            Player.Disengage(_mockApi);
            // Verify outcome
            Assert.Null(_mockApi.Mock.Windower.LastCommand);
            // Teardown	
        }

        [Fact]
        public void EngageWhenStandingWillAttack()
        {
            // Fixture setup            
            _mockApi.Mock.Player.Status = Status.Standing;
            // Excercise system
            Player.Engage(_mockApi);
            // Verify outcome
            Assert.Equal(Constants.AttackTarget, _mockApi.Mock.Windower.LastCommand);
            // Teardown	
        }

        [Fact]
        public void EngageWhenAlreadyAttackingDoesNothing()
        {
            // Fixture setup
            _mockApi.Mock.Player.Status = Status.Fighting;
            // Excercise system
            Player.Engage(_mockApi);
            // Verify outcome
            Assert.Null(_mockApi.Mock.Windower.LastCommand);
            // Teardown	
        }

        [Fact]
        public void SetTargetChangesMobIDAndPlacesCursorOnTarget()
        {
            // Fixture setup            
            _mockApi.Mock.Player.Status = Status.Standing;
            // Excercise system
            const int anyUnitId = 100;
            Player.SetTarget(_mockApi, new MockUnit() { Id = anyUnitId });
            // Verify outcome
            Assert.Equal(anyUnitId, _mockApi.Mock.Target.LastTargetID);
            Assert.Equal(Constants.SetTargetCursor, _mockApi.Mock.Windower.LastCommand);
            // Teardown	
        }

        [Fact]
        public void RestWhenNotHealingWillRest()
        {
            // Fixture setup            
            _mockApi.Mock.Player.Status = Status.Standing;
            // Excercise system
            Player.Rest(_mockApi);
            // Verify outcome
            Assert.Equal(Constants.RestingOn, _mockApi.Mock.Windower.LastCommand);
            // Teardown	
        }

        [Fact]
        public void RestWhenHealingDoesNothing()
        {
            // Fixture setup            
            _mockApi.Mock.Player.Status = Status.Healing;
            // Excercise system
            Player.Rest(_mockApi);
            // Verify outcome
            Assert.Null(_mockApi.Mock.Windower.LastCommand);
            // Teardown	
        }

        [Fact]
        public void StandWhenNotStandingWillStandUp()
        {
            // Fixture setup            
            _mockApi.Mock.Player.Status = Status.Healing;
            // Excercise system
            Player.Stand(_mockApi);
            // Verify outcome
            Assert.Equal(Constants.RestingOff, _mockApi.Mock.Windower.LastCommand);
            // Teardown	
        }

        [Fact]
        public void StandWhenStandingDoesNothing()
        {
            // Fixture setup            
            _mockApi.Mock.Player.Status = Status.Standing;
            // Excercise system
            Player.Stand(_mockApi);
            // Verify outcome
            Assert.Null(_mockApi.Mock.Windower.LastCommand);
            // Teardown	
        }

        [Fact]
        public void StopRunningWhenRunningWillStopRunning()
        {
            // Fixture setup            
            _mockApi.Mock.Navigator.IsRunning = true;
            // Excercise system
            Player.StopRunning(_mockApi);
            // Verify outcome
            Assert.False(_mockApi.Mock.Navigator.IsRunning);
            // Teardown	
        }
    }
}
