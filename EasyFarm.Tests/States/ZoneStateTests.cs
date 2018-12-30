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
using EasyFarm.Context;
using EasyFarm.States;
using EasyFarm.Tests.Context;
using MemoryAPI;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class ZoneStateTests
    {
        private static readonly Zone StartingZone = Zone.Konschtat_Highlands;
        private static readonly Zone NewZone = Zone.Valkurm_Dunes;

        public class CheckTests
        {
            [Fact]
            public void CheckIsTrueWhenZoneChanges()
            {
                // Fixture setup
                var context = new TestContext();
                context.Player.Zone = StartingZone;
                context.Zone = NewZone;
                context.Player.Str = 100;
                var sut = CreateSut();
                // Exercise system
                var result = sut.Check(context);
                // Verify outcome
                Assert.True(result);
                // Teardown
            }

            [Fact]
            public void CheckIsTrueWhenPlayersStatsAreZero()
            {
                // Fixture setup
                var context = new TestContext();
                context.Player.Str = 0;
                var sut = CreateSut();
                // Exercise system
                var result = sut.Check(context);
                // Verify outcome
                Assert.True(result);
                // Teardown
            }

            [Fact]
            public void CheckIsFalseWhenNotZoning()
            {
                // Fixture setup
                var context = new TestContext();
                context.Player.Zone = StartingZone;
                context.Player.Str = 100;
                context.Zone = StartingZone;
                var sut = CreateSut();
                // Exercise system
                var result = sut.Check(context);
                // Verify outcome
                Assert.False(result);
                // Teardown
            }
        }

        public class RunTests
        {
            [Fact]
            public void RunOnZoningSetsZoneToNewZone()
            {
                // Fixture setup
                var context = new TestContext();
                context.Player.Zone = NewZone;
                context.Player.Str = 100;
                var sut = CreateSut();
                // Exercise system
                sut.Run(context);
                // Verify outcome
                Assert.Equal(NewZone, context.Zone);
                // Teardown
            }

            [Fact]
            public void RunOnZoningStopsPlayerFromRunning()
            {
                // Fixture setup
                var context = new TestContext();
                context.Player.Zone = NewZone;
                var sut = CreateSut(zoningAction: () => ForceMoveToNextZone(context));
                // Exercise system
                sut.Run(context);
                // Verify outcome
                Assert.False(context.MockAPI.Navigator.IsRunning);
                // Teardown
            }

            [Fact]
            public void RunWhileZoningWaits()
            {
                // Fixture setup
                var context = new TestContext();
                context.Zone = StartingZone;
                context.Player.Str = 0;
                context.Player.Zone = NewZone;
                var sut = CreateSut(() => ForceMoveToNextZone(context));
                // Exercise system
                sut.Run(context);
                // Verify outcome
                Assert.Equal(100, context.Player.Str);
                // Teardown
            }

            private void ForceMoveToNextZone(IGameContext context)
            {
                context.Player.Str = 100;
            }
        }

        private static ZoneState CreateSut(Action zoningAction = null)
        {
            var sut = new ZoneState();
            sut.ZoningAction = zoningAction ?? sut.ZoningAction;
            return sut;
        }
    }
}
