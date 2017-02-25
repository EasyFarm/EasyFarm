using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MemoryAPI.Navigation;

namespace MemoryAPI.Tests
{
    public class FakePlayer : IPlayerTools
    {
        private static IEnumerator<float> _castPercentExEnumerator = 
            CastPercentExValues()
            .GetEnumerator();

        public float CastPercentEx
        {
            get
            {
                var value = _castPercentExEnumerator.Current;
                var result = _castPercentExEnumerator.MoveNext();
                if (!result) _castPercentExEnumerator = CastPercentExValues().GetEnumerator();
                return value;
            }
        }
        public int HPPCurrent { get; set; }
        public int ID { get; set; }
        public int MPCurrent { get; set; }
        public int MPPCurrent { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public Structures.PlayerStats Stats { get; set; }
        public Status Status { get; set; }
        public StatusEffect[] StatusEffects { get; set; }
        public int TPCurrent { get; set; }
        public Zone Zone { get; set; }

        public static IEnumerable<float> CastPercentExValues()
        {
            yield return 0;
            yield return 0;
            yield return 0;

            yield return 50;
            yield return 50;
            yield return 50;

            yield return 100;
            yield return 100;
            yield return 100;

            var values = Enumerable.Range(0, 100)
                .Select(x => 100f)
                .ToList();

            foreach (var value in values)
            {
                yield return value;
            }
        }
    }
}
