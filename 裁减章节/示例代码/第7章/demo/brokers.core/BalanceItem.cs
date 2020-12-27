namespace TraderBot.Core
{
    public class BalanceItem
    {
        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public decimal Available { get; set; }

        public decimal Pending { get; set; }
    }
}
