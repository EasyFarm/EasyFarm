namespace EasyFarm.Tests.Classes
{
    public class CastTime
    {
        private readonly int _castTime;

        public CastTime(int castTime)
        {
            _castTime = castTime;
        }

        public bool IsCasted => _castTime == 100;

        public int Value => (this);

        public static explicit operator CastTime(int castTime) => new CastTime(castTime);

        public static implicit operator int(CastTime castTime) => castTime.Value;
    }
}
