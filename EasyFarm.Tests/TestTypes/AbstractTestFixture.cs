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
using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.Tests.TestTypes
{
    public class AbstractTestFixture
    {
        public static BattleAbility FindAbility()
        {
            var battleAbility = new BattleAbility();
            battleAbility.IsEnabled = true;
            battleAbility.Name = "valid";
            return battleAbility;
        }

        public static IUnit FindNonValidUnit()
        {
            return new FakeUnit();
        }

        public static IUnit FindUnit()
        {
            var unit = new FakeUnit
            {
                Name = "Mandragora",
                ClaimedId = 0,
                Distance = 3.0,
                HasAggroed = false,
                HppCurrent = 100,
                Id = 200,
                IsActive = true,
                IsClaimed = false,
                IsDead = false,
                IsPet = false,
                IsRendered = true,
                MyClaim = false,
                NpcType = NpcType.Mob,
                PartyClaim = false,
                Status = Status.Standing,
                YDifference = 2.0
            };
            return unit;
        }

        public static FakePlayer FindPlayer()
        {
            var player = new FakePlayer();
            player.HPPCurrent = 100;
            player.MPCurrent = 10000;
            player.MPPCurrent = 100;
            player.Name = "Mykezero";
            player.Status = Status.Standing;
            player.TPCurrent = 1000;
            player.StatusEffects = new StatusEffect[] { };
            return player;
        }

        public static FakeTimer FindTimer()
        {
            var timer = new FakeTimer();
            return timer;
        }

        public static FakeWindower FindWindower()
        {
            return new FakeWindower();
        }

        public static FakeNavigator FindNavigator()
        {
            return new FakeNavigator();
        }

        public static FakeTarget FindTarget()
        {
            return new FakeTarget();
        }
    }
}