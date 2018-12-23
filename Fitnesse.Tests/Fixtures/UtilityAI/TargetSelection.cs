using System;
using fit;

namespace Fitnesse.Tests.Fixtures.UtilityAI
{
    public class TargetSelection : ColumnFixture
    {
        public decimal IsHoldingGrenade { get; set; }
        public decimal SniperAngle { get; set; }
        public decimal GunnerDistance { get; set; }
        public decimal GrenadierDistance { get; set; }
        public decimal SniperDistance { get; set; }

        public decimal Grenadier()
        {
            return 0;
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
