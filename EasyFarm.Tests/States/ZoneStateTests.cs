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

using System;
using EasyFarm.States;
using EasyFarm.Tests.TestTypes;
using EasyFarm.Tests.TestTypes.Mocks;
using MemoryAPI;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class ZoneStateTests
    {
        private static readonly Zone StartingZone = Zone.Konschtat_Highlands;
        private static readonly Zone NewZone = Zone.Valkurm_Dunes;

        public class CheckTests : AbstractTestBase
        {
            [Fact]
            public void CheckIsTrueWhenZoneChanges()
            {
                // Fixture setup
                MockGameAPI.Mock.Player.Zone = StartingZone;
                var sut = CreateSut(new StateMemory(MockGameAPI)
                {
                    Config = MockConfig,
                    UnitFilters = new MockUnitFilters()
                });
                MockGameAPI.Mock.Player.Zone = NewZone;
                MockGameAPI.Mock.Player.Stats = new Structures.PlayerStats { Str = 100 };
                // Exercise system
                var result = sut.Check();
                // Verify outcome
                Assert.True(result);
                // Teardown
            }

            [Fact]
            public void CheckIsTrueWhenPlayersStatsAreZero()
            {
                // Fixture setup
                MockGameAPI.Mock.Player.Stats = new Structures.PlayerStats() { Str = 0 };
                var sut = CreateSut(new StateMemory(MockGameAPI)
                {
                    Config = MockConfig,
                    UnitFilters = new MockUnitFilters()
                });
                // Exercise system
                var result = sut.Check();
                // Verify outcome
                Assert.True(result);
                // Teardown
            }

            [Fact]
            public void CheckIsFalseWhenNotZoning()
            {
                // Fixture setup
                MockGameAPI.Mock.Player.Zone = StartingZone;
                MockGameAPI.Mock.Player.Stats = new Structures.PlayerStats() { Str = 100 };
                var sut = CreateSut(new StateMemory(MockGameAPI)
                {
                    Config = MockConfig,
                    UnitFilters = new MockUnitFilters()
                });
                // Exercise system
                var result = sut.Check();
                // Verify outcome
                Assert.False(result);
                // Teardown
            }
        }

        public class RunTests : AbstractTestBase
        {
            [Fact]
            public void RunOnZoningSetsZoneToNewZone()
            {
                // Fixture setup
                MockGameAPI.Mock.Player.Zone = StartingZone;
                MockGameAPI.Mock.Player.Stats = new Structures.PlayerStats() { Str = 100 };
                var sut = CreateSut(new StateMemory(MockGameAPI)
                {
                    Config = MockConfig,
                    UnitFilters = new MockUnitFilters()
                });
                MockGameAPI.Mock.Player.Zone = NewZone;
                // Exercise system
                sut.Run();
                // Verify outcome
                Assert.Equal(NewZone, sut.Zone);
                // Teardown
            }

            [Fact]
            public void RunOnZoningStopsPlayerFromRunning()
            {
                // Fixture setup
                var sut = CreateSut(new StateMemory(MockGameAPI)
                {
                    Config = MockConfig,
                    UnitFilters = new MockUnitFilters()
                }, zoningAction: ForceMoveToNextZone);
                MockGameAPI.Mock.Player.Zone = NewZone;
                // Exercise system
                sut.Run();
                // Verify outcome
                Assert.False(MockGameAPI.Mock.Navigator.IsRunning);
                // Teardown
            }

            [Fact]
            public void RunWhileZoningWaits()
            {
                // Fixture setup
                MockGameAPI.Mock.Player.Zone = StartingZone;
                MockGameAPI.Mock.Player.Stats = new Structures.PlayerStats() { Str = 0 };
                var sut = CreateSut(new StateMemory(MockGameAPI)
                {
                    Config = MockConfig,
                    UnitFilters = new MockUnitFilters()
                }, zoningAction: ForceMoveToNextZone);
                MockGameAPI.Mock.Player.Zone = NewZone;
                // Exercise system
                sut.Run();
                // Verify outcome
                Assert.Equal(100, MockGameAPI.Mock.Player.Stats.Str);
                // Teardown
            }

            private void ForceMoveToNextZone()
            {
                MockGameAPI.Mock.Player.Stats = new Structures.PlayerStats { Str = 100 };
            }
        }

        private static ZoneState CreateSut(StateMemory stateMemory, Action zoningAction = null)
        {
            var sut = new ZoneState(stateMemory);
            sut.ZoningAction = zoningAction ?? sut.ZoningAction;
            return sut;
        }
    }
}
