using System;
using fit;

namespace Fitnesse.Tests.Fixtures.UtilityAI
{
    public class TargetSelection : ColumnFixture
    {
        private decimal _grenadierScore;
        private decimal _sniperScore;

        private void ComputeScores()
        {
            var world = SetupWorld();
            CalculateGrenadierScore(world);
            CalculateSniperScore(world);
        }

        private void CalculateSniperScore(GameWorld world)
        {
            decimal angleScore = new IsSniperPointingTowardsUs(world).Score();
            decimal proximityScore = new GameObjectProximityScorer(world).Score();
            _sniperScore = angleScore + proximityScore;
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
                GrenadierDistance = GrenadierDistance,
                SniperAngle = SniperAngle,
                SniperDistance = SniperDistance
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
            ComputeScores();
            return _sniperScore;
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
