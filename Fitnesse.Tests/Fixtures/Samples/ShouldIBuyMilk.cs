using fit;

namespace Fitnesse.Tests
{
    public class ShouldIBuyMilk : ColumnFixture
    {
        public int CashInWallet;
        public int PintsOfMilkRemaining;
        public string CreditCard;

        public string GoToStore()
        {
            if (CashInWallet > 0 || CreditCard.Equals("yes"))
                return "yes";
            return "no";
        }
    }
}
