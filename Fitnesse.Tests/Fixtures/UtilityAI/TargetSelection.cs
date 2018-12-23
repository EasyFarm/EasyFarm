using System;
using fit;

namespace Fitnesse.Tests.Fixtures.UtilityAI
{
    public class TargetSelection : ColumnFixture
    {
        private decimal _grenadierScore;        

        public void Execute()
        {
            var world = new GameWorld()
            {
                IsHoldingGrenade = IsHoldingGrenade
            };
            var scorer = new IsGrenadierHoldingGrenade(world);
            _grenadierScore = scorer.Score();
        }

        public bool IsHoldingGrenade { get; set; }
        public decimal SniperAngle { get; set; }
        public decimal GunnerDistance { get; set; }
        public decimal GrenadierDistance { get; set; }
        public decimal SniperDistance { get; set; }


        public decimal Grenadier()
        {
            Execute();
            return _grenadierScore;
        }

        public decimal Sniper()
        {
            return 0;
        }

        public decimal Gunner()
        {
            return 0;
        }

        public String Result()
        {
            return "None";
        }
    }

    public class IsGrenadierHoldingGrenade
    {
        private readonly GameWorld _world;

        private decimal _score = 50;

        public IsGrenadierHoldingGrenade(GameWorld world)
        {
            _world = world;
        }

        public decimal Score()
        {
            return _world.IsHoldingGrenade ? _score : 0;
        }
    }

    public class GameWorld
    {
        public bool IsHoldingGrenade { get; set; }
    }
}
