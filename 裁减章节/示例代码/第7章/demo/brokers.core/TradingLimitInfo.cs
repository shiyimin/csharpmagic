namespace TraderBot.Core
{
    public class TradingLimitInfo
    {
        public string Quote { get; set; }

        public string Coin { get; set; }

        public decimal MinimumOrderSize { get; set; }

        public decimal MaximumOrderSize { get; set; }

        public int VolumePrecision { get; set; } = 3;

        public int QuotePrecision { get; set; } = 8;

        public int CoinPrecision { get; set; } = 8;
    }
}
