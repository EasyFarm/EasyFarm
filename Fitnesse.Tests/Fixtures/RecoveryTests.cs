using System;
using fit;

namespace Fitnesse.Tests.Fixtures
{
    public class RecoveryTests : ColumnFixture
    {
        // World
        public bool HasAggro { get; set; }
        public bool IsFighting { get; set; }
        public String StatusEffect { get; set; }
        public int HppCurrent { get; set; }   
        public int MppCurrent { get; set; }
        public bool HasHealingMove { get; set; }

        // Config
        public bool IsMagicEnabled { get; set; }
        public bool IsHealthEnabled { get; set; }
        public int HighHealth { get; set; }
        public int LowHealth { get; set; }
        public int HighMagic { get; set; }
        public int LowMagic { get; set; }

        // Result
        public float RestingScore { get; set; }
        public float HealingScore { get; set; }
    }
}
