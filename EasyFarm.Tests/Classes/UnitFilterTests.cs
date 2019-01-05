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
using MemoryAPI.Navigation;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class UnitFilterTests
    {
        private MockUnit mob;
        private IUnitFilters sut;
        private IConfig config;
        private MockGameAPI api;

        public UnitFilterTests()
        {
            mob = FindBasicMob();
            sut = new UnitFilters();
            config = FindConfig();
            api = new MockGameAPI();
        }

        private static MockConfig FindConfig()
        {
            return new MockConfig()
            {
                DetectionDistance = 10
            };
        }

        [Fact]
        public void Filter_BasicMob()
        {
            Assert.True(sut.MobFilter(api, mob, config));
        }

        [Fact]
        public void Filter_NullMemoryApi()
        {
            Assert.False(sut.MobFilter(null, mob, config));
        }

        [Fact]
        public void Filter_NullMob()
        {
            Assert.False(sut.MobFilter(api, null, config));
        }

        [Fact]
        public void Filter_IsActive()
        {
            mob.IsActive = false;
            Assert.False(sut.MobFilter(api, mob, config));
        }

        [Fact]
        public void Filter_IsDead()
        {
            mob.IsDead = true;
            Assert.False(sut.MobFilter(api, mob, config));
        }

        [Fact]
        public void Filter_IsRendered()
        {
            mob.IsRendered = false;
            Assert.False(sut.MobFilter(api, mob, config));
        }

        [Theory]
        [InlineData(NpcType.PC)]
        [InlineData(NpcType.InanimateObject)]
        [InlineData(NpcType.NPC)]
        [InlineData(NpcType.Self)]
        public void Filter_MobType(NpcType npcType)
        {
            mob.NpcType = npcType;
            Assert.False(sut.MobFilter(api, mob, config));
        }

        [Fact]
        public void Filter_IsPet()
        {
            mob.IsPet = true;
            Assert.False(sut.MobFilter(api, mob, config));
        }

        [Fact]
        public void Filter_DetectionDistance()
        {
            config.DetectionDistance = 3;
            mob.Distance = 4;
            Assert.False(sut.MobFilter(api, mob, config));
        }

        [Fact]
        public void Filter_WanderDistance()
        {
            config.WanderDistance = 1;
            mob.Position = new Position() { X = 1, Z = 1 };
            config.Route.Waypoints.Add(new Position(){ X = 3, Z = 3 });
            Assert.False(sut.MobFilter(api, mob, config));
        }

        [Fact]
        public void Filter_HeightThreshold()
        {
            mob.YDifference = 10;
            config.HeightThreshold = 5;
            Assert.False(sut.MobFilter(api, mob, config));
        }

        [Fact]
        public void Filter_IgnoredMobs()
        {
            mob.Name = "Ignored";
            config.IgnoredMobs.Add("Ignored");
            Assert.False(sut.MobFilter(api, mob, config));
        }

        [Fact]
        public void Filter_AggroFilter()
        {
            MakeMobInvalid();
            mob.HasAggroed = false;
            config.AggroFilter = true;
            Assert.False(sut.MobFilter(api, mob, config));
        }

        [Fact]
        public void Filter_TargetedMobs()
        {

        }

        [Fact]
        public void Filter_PartyFilter()
        {

        }

        [Fact]
        public void Filter_UnclaimedFilter()
        {

        }

        [Fact]
        public void Filter_ClaimedFilter()
        {

        }

        [Fact]
        public void Filter_ClaimID_OurClaim()
        {
            mob.ClaimedId = 5000;
            api.Mock.PartyMember[0].ServerID = 10000;
            Assert.False(sut.MobFilter(api, mob, config));
        }


        private void MakeMobInvalid()
        {
            config.TargetedMobs.Add("Targeted");
            mob.Name = "Mandragora";
        }

        private static MockUnit FindBasicMob()
        {
            var mob = new MockUnit()
            {
                IsActive = true,
                ClaimedId = 0,
                IsRendered = true,
                NpcType = NpcType.Mob,
                IsPet = false,
            };
            return mob;
        }
    }
}
