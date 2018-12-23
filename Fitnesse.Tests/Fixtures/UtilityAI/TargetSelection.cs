using System;
using fit;

namespace Fitnesse.Tests.Fixtures.UtilityAI
{
    public class TargetSelection : ColumnFixture
    {
        private decimal _grenadierScore;

        private void ComputeScores()
        {
            var world = SetupWorld();
            CalculateGrenadierScore(world);
        }

        private void CalculateGrenadierScore(GameWorld world)
        {
            var hasGrenadeScorer = new IsGrenadierHoldingGrenade(world);
            var proximityScorer = new GameObjectProximityScorer(world);
            _grenadierScore = hasGrenadeScorer.Score() + proximityScorer.Score();
        }

        private GameWorld SetupWorld()
        {
            var world = new GameWorld()
            {
                IsHoldingGrenade = IsHoldingGrenade,
                GrenadierDistance = GrenadierDistance
            };
            return world;
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
}
