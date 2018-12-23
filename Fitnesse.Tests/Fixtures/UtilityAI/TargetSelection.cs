using System;
using fit;

namespace Fitnesse.Tests.Fixtures.UtilityAI
{
    public class TargetSelection : ColumnFixture
    {
        private decimal _grenadierScore;

        private void ComputeScores()
        {
            var world = new GameWorld()
            {
                IsHoldingGrenade = IsHoldingGrenade,
                GrenadierDistance = GrenadierDistance
            };
            var hasGrenadeScorer = new IsGrenadierHoldingGrenade(world);
            var proximityScorer = new GameObjectProximityScorer(world);
            _grenadierScore = hasGrenadeScorer.Score() + proximityScorer.Score();
        }

        public bool IsHoldingGrenade { get; set; }
        public decimal SniperAngle { get; set; }
        public decimal GunnerDistance { get; set; }
        public decimal GrenadierDistance { get; set; }
        public decimal SniperDistance { get; set; }


        public decimal Grenadier()
        {
            ComputeScores();
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

    public class GameWorld
    {
        public bool IsHoldingGrenade { get; set; }
        public decimal GrenadierDistance { get; set; }
    }
}
