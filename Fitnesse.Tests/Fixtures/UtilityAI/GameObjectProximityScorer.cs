using System;

namespace Fitnesse.Tests.Fixtures.UtilityAI
{
    public class GameObjectProximityScorer
    {
        private readonly GameWorld _world;
        private readonly decimal score = 30;
        private readonly decimal weight = 1;

        public GameObjectProximityScorer(GameWorld world)
        {
            _world = world;
        }
 
        public decimal Score()
        {
            decimal distance = _world.GrenadierDistance;
            return Math.Max(0, (score - distance) * weight);
        }
    }
}