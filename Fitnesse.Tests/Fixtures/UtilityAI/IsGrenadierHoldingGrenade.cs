namespace Fitnesse.Tests.Fixtures.UtilityAI
{
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
}