using System.Collections.Generic;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.Tests.TestTypes
{
    public class FakePlayer : IPlayerTools
    {
        private static readonly IEnumerator<float> CastPercentExEnumerator = 
            CastPercentExValues()
            .GetEnumerator();

        public float CastPercentEx
        {
            get
            {
                var value = CastPercentExEnumerator.Current;
                CastPercentExEnumerator.MoveNext();
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
        public Job Job { get; set; }
        public Job SubJob { get; set; }


        public static IEnumerable<float> CastPercentExValues()
        {
            yield return 0;
            yield return 0;
            yield return 0;

            yield return 50;
            yield return 50;
            yield return 50;

            while (true)
            {
                yield return 100;
            }           
        }
    }
}
