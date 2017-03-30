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