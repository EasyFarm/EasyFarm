namespace Fitnesse.Tests.Fixtures.UtilityAI
{
    public sealed class IsSniperPointingTowardsUs
    {
        private readonly GameWorld _world;
        private readonly decimal score = 40;
        private readonly decimal maxAngle = 30;

        public IsSniperPointingTowardsUs(GameWorld world)
        {
            _world = world;
        }

        public decimal Score()
        {
            decimal angle = _world.SniperAngle;
            return angle < maxAngle ? score : 0;
        }
    }
}