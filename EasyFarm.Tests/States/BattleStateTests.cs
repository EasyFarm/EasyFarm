// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
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

using System.Collections.ObjectModel;
using EasyFarm.Classes;
using EasyFarm.Parsing;
using EasyFarm.States;
using EasyFarm.Tests.TestTypes;
using EasyFarm.Tests.TestTypes.Mocks;
using MemoryAPI;
using MemoryAPI.Memory;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class Check : AbstractTestBase
    {
        private StateMemory CreateStateMemory()
        {
            StateMemory memory = new StateMemory(new MockEliteAPIAdapter(MockEliteAPI));
            return memory;
        }

        [Fact]
        public void WhenEngagedSetAndEngagedShouldBattle()
        {
            // Fixture setup
            Config.IsEngageEnabled = true;
            MockEliteAPI.Player.Status = Status.Fighting;
            StateMemory memory = CreateStateMemory();
            memory.Target = FindUnit();
            memory.IsFighting = true;
            BattleState sut = new BattleState(memory);
            // Exercise system
            bool result = sut.Check();
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void WhenEngagedNotSetShouldBattle()
        {
            // Fixture setup
            Config.IsEngageEnabled = false;
            StateMemory memory = CreateStateMemory();
            memory.Target = FindUnit();
            memory.IsFighting = true;
            BattleState sut = new BattleState(memory);
            // Exercise system
            bool result = sut.Check();
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void WithNonValidUnitShouldNotBattle()
        {
            // Fixture setup
            Config.IsEngageEnabled = false;
            StateMemory memory = CreateStateMemory();
            memory.Target = FindNonValidUnit();
            memory.IsFighting = true;
            BattleState sut = new BattleState(memory);
            // Exercise system
            bool result = sut.Check();
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void WhenNotFightingShouldntBattle()
        {
            // Fixture setup
            Config.IsEngageEnabled = false;
            StateMemory memory = CreateStateMemory();
            memory.Target = FindUnit();
            memory.IsFighting = false;
            BattleState sut = new BattleState(memory);
            // Exercise system
            bool result = sut.Check();
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void WhenInjuredShouldntBattle()
        {
            // Fixture setup
            StateMemory memory = CreateStateMemory();
            memory.Target = FindUnit();
            memory.IsFighting = false;
            MockEliteAPI.Player.HPPCurrent = 25;
            MockEliteAPI.Player.Status = Status.Standing;
            Config.LowHealth = 50;
            Config.HighHealth = 100;
            Config.IsHealthEnabled = true;
            Config.IsEngageEnabled = false;
            BattleState sut = new BattleState(memory);
            // Exercise system
            bool result = sut.Check();
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void WithInvalidTargetShouldntBattle()
        {
            // Fixture setup
            StateMemory memory = CreateStateMemory();
            memory.Target = FindNonValidUnit();
            memory.IsFighting = true;
            MockEliteAPI.Player.Status = Status.Standing;
            Config.IsEngageEnabled = false;
            BattleState sut = new BattleState(memory);
            // Exercise system
            bool result = sut.Check();
            // Verify outcome
            Assert.False(result);
            // Teardown
        }
    }

    public class Run : AbstractTestBase
    {
        private BattleState CreateSut()
        {
            var memory = new StateMemory(new MockEliteAPIAdapter(MockEliteAPI)) {Target = FindUnit()};
            BattleState sut = new BattleState(memory);
            return sut;
        }

        [Fact]
        public void WithValidActionWillSendCommand()
        {
            // Fixture setup
            BattleState sut = CreateSut();
            ObservableCollection<BattleAbility> actions = FindBattleActions();
            BattleAbility ability = FindJobAbility("test");
            ability.Command = "/jobability test <me>";
            actions.Add(ability);
            // Exercise system
            sut.Run();
            // Verify outcome
            Assert.Equal("/jobability test <me>", MockEliteAPI.Windower.LastCommand);
            // Teardown
        }

        [Fact]
        public void WithInvalidActionWillNotSendCommand()
        {
            // Fixture setup
            BattleState sut = CreateSut();
            ObservableCollection<BattleAbility> actions = FindBattleActions();
            BattleAbility ability = FindJobAbility("test");
            ability.IsEnabled = false;
            actions.Add(ability);
            // Exercise system
            sut.Run();
            // Verify outcome
            Assert.Null(MockEliteAPI.Windower.LastCommand);
        }

        private static BattleAbility FindJobAbility(string actionName)
        {
            BattleAbility battleAbility = FindAbility();
            battleAbility.Name = actionName;
            battleAbility.AbilityType = AbilityType.Jobability;
            battleAbility.TargetType = TargetType.Self;
            return battleAbility;
        }

        private ObservableCollection<BattleAbility> FindBattleActions()
        {
            ObservableCollection<BattleAbility> moves = Config.BattleLists["Battle"].Actions;
            moves.Clear();
            return moves;
        }
    }

    public class Enter : AbstractTestBase
    {
        [Fact]
        public void WithHealingPlayerWillStandUp()
        {
            // Fixture setup
            BattleState sut = CreateSut();
            MockEliteAPI.Player.Status = Status.Healing;
            // Exercise system
            sut.Enter();
            // Verify outcome
            Assert.Equal(Status.Standing, MockEliteAPI.Player.Status);
            // Teardown
        }

        [Fact]
        public void WillStopPlayerFromMoving()
        {
            // Fixture setup
            MockEliteAPI.Navigator.IsRunning = true;
            BattleState sut = CreateSut();
            // Exercise system
            sut.Enter();
            // Verify outcome
            Assert.False(MockEliteAPI.Navigator.IsRunning);
            // Teardown
        }

        private BattleState CreateSut()
        {
            return new BattleState(new StateMemory(new MockEliteAPIAdapter(MockEliteAPI)));
        }
    }
}
