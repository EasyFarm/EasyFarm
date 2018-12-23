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
            var sniperWorld = SetupWorld();
            var grenadierWorld = SetupWorld();
            var sniper = new Sniper()
            {
                Angle = sniperWorld.SniperAngle,
                Distance = sniperWorld.SniperDistance
            };
            var grenadier = new Grenadier()
            {
                IsHoldingGrenade = grenadierWorld.IsHoldingGrenade,
                Distance = grenadierWorld.GrenadierDistance
            };
            CalculateGrenadierScore(grenadierWorld, grenadier);
            CalculateSniperScore(sniperWorld, sniper);
        }

        private void CalculateSniperScore(GameWorld world, Sniper sniper)
        {
            world.GrenadierDistance = sniper.Distance;
            decimal angleScore = new IsSniperPointingTowardsUs(world).Score();
            decimal proximityScore = new GameObjectProximityScorer(world).Score();
            _sniperScore = angleScore + proximityScore;
        }

        private void CalculateGrenadierScore(GameWorld world, Grenadier grenadier)
        {
            world.GrenadierDistance = grenadier.Distance;
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
