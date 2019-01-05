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

using System.Threading;
using EasyFarm.States;
using EasyFarm.Tests.Context;
using EasyFarm.UserSettings;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class FiniteStateMachineTests
    {
        private readonly TestContext _context = new TestContext();
        private readonly NewFiniteStateMachine _sut = CreateSut();

        [Fact]
        public void OnFirstRun_StartStateWillRun()
        {
            // Fixture setup
            // Exercise system
            StateHistory result = _sut.Run(_context, new CancellationTokenSource());
            // Verify outcome
            bool checkRan = result.HasCheck(typeof(StartEngineState));
            Assert.True(checkRan);
            // Teardown	
        }

        private static NewFiniteStateMachine CreateSut()
        {
            NewFiniteStateMachine sut = new NewFiniteStateMachine();

            //Create the states
            sut.AddState(new DeadState() {Priority = 7});
            sut.AddState(new ZoneState() {Priority = 7});
            sut.AddState(new SetTargetState() {Priority = 7});
            sut.AddState(new SetFightingState() {Priority = 7});
            sut.AddState(new FollowState() {Priority = 5});
            sut.AddState(new RestState() {Priority = 2});
            sut.AddState(new SummonTrustsState() {Priority = 6});
            sut.AddState(new ApproachState() {Priority = 0});
            sut.AddState(new BattleState() {Priority = 3});
            sut.AddState(new WeaponskillState() {Priority = 2});
            sut.AddState(new PullState() {Priority = 4});
            sut.AddState(new StartState() {Priority = 5});
            sut.AddState(new TravelState() {Priority = 1});
            sut.AddState(new HealingState() {Priority = 2});
            sut.AddState(new EndState() {Priority = 3});
            sut.AddState(new StartEngineState() {Priority = Constants.MaxPriority});

            sut.IsDryRun = true;

            return sut;
        }
    }
}
