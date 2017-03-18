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
            var sut = new ZoneState(api);
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
            var sut = new ZoneState(api);;            

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
            var sut = new ZoneState(api); ;

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
            var sut = new ZoneState(api);

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
            var sut = new ZoneState(api);

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
            var sut = new ZoneState(api)
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
