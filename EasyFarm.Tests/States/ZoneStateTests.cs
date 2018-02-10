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
using EasyFarm.States;
using EasyFarm.Tests.TestTypes;
using MemoryAPI;
using Moq;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class ZoneStateTests
    {
        [Fact]
        public void CheckIsTrueWhenZoneChanges()
        {
            // Fixture setup
            var api = new FakeMemoryAPI();
            var player = new FakePlayer { Zone = Zone.Konschtat_Highlands};
            api.Player = player;
            var sut = new ZoneState(new StateMemory(api));
            player.Zone = Zone.Valkurm_Dunes;
            player.Stats = new Structures.PlayerStats { Str = 100 };

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
            var api = new FakeMemoryAPI();
            var player = new FakePlayer
            {
                Zone = Zone.Konschtat_Highlands,
                Stats = new Structures.PlayerStats {Str = 0}
            };
            api.Player = player;            
            var sut = new ZoneState(new StateMemory(api));

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
            var api = new FakeMemoryAPI();
            var player = new FakePlayer
            {
                Zone = Zone.Konschtat_Highlands,
                Stats = new Structures.PlayerStats { Str = 100 }
            };
            api.Player = player;
            var sut = new ZoneState(new StateMemory(api));

            // Exercise system
            var result = sut.Check();

            // Verify outcome
            Assert.False(result);

            // Teardown
        }

        [Fact]
        public void RunOnZoningSetsZoneToNewZone()
        {
            // Fixture setup
            var api = new FakeMemoryAPI();
            var player = new FakePlayer
            {
                Zone = Zone.Konschtat_Highlands,
                Stats = new Structures.PlayerStats { Str = 100 }
            };

            var navigation = new Mock<INavigatorTools>();
            api.Player = player;
            api.Navigator = navigation.Object;
            var sut = new ZoneState(new StateMemory(api));

            player.Zone = Zone.Valkurm_Dunes;

            // Exercise system
            sut.Run();

            // Verify outcome
            Assert.Equal(Zone.Valkurm_Dunes, sut.Zone);

            // Teardown
        }

        [Fact]
        public void RunOnZoningStopsPlayerFromRunning()
        {
            // Fixture setup
            var api = new FakeMemoryAPI();
            var player = new FakePlayer
            {
                Zone = Zone.Konschtat_Highlands,
                Stats = new Structures.PlayerStats { Str = 100 }
            };

            var navigation = new Mock<INavigatorTools>();
            navigation.Setup(x => x.Reset());
            api.Player = player;
            api.Navigator = navigation.Object;
            var sut = new ZoneState(new StateMemory(api));

            player.Zone = Zone.Valkurm_Dunes;

            // Exercise system
            sut.Run();

            // Verify outcome
            navigation.Verify(x => x.Reset(), Times.Once());

            // Teardown
        }

        [Fact]
        public void RunWhileZoningWaits()
        {
            // Fixture setup
            var api = new FakeMemoryAPI();
            var player = new FakePlayer
            {
                Zone = Zone.Konschtat_Highlands,
                Stats = new Structures.PlayerStats { Str = 0 }
            };

            var navigation = new Mock<INavigatorTools>();
            navigation.Setup(x => x.Reset());
            api.Player = player;
            api.Navigator = navigation.Object;
            var sut = new ZoneState(new StateMemory(api))
            {
                ZoningAction = () =>
                {
                    player.Stats = new Structures.PlayerStats
                    {
                        Str = 100
                    };
                }
            };

            player.Zone = Zone.Valkurm_Dunes;

            // Exercise system
            sut.Run();

            // Verify outcome
            Assert.Equal(100, player.Stats.Str);

            // Teardown
        }
    }
}
