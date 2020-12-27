using System;

namespace TraderBot.Core
{
    public class Ticker
    {
        public string Site { get; set; }

        public string Quote { get; set; }

        public string Coin { get; set; }

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public decimal? CoinVolume { get; set; }

        public DateTime Timestamp { get; set; }

        public decimal QuoteVolume { get; set; }
    }
}
